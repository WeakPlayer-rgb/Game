using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.PropertyGridInternal;
using static System.Drawing.Bitmap;

// ReSharper disable All

namespace NewGame
{
    public sealed partial class VisualisationAndController : Form
    {
        private GameModel gameModel;
        private bool isWdown = false;
        private bool isAdown = false;
        private bool isDdown = false;
        private bool isSdown = false;
        private Dictionary<string, Image> images;
        private Image grass;
        private Player localPlayer;


        public VisualisationAndController(GameModel g)
        {
            KeyPreview = true;
            DoubleBuffered = true;
            gameModel = g;
            images = new Dictionary<string, Image>();
            localPlayer = gameModel.Player;
            localPlayer.CoolDown = 10;
            var coolDownShot = 0;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            var dir = new DirectoryInfo(path);

            foreach (var file in dir.GetFiles())
                images[file.Name] = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Images", file.Name));

            Activate();
            var bmp = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Images", "grass.png"));
            var Grass = CreateColumn(CreatLine(bmp, ClientSize.Width, ClientSize.Height), ClientSize.Width,
                ClientSize.Height);
            var labelX = new Label {Location = new Point(0, 0), Width = 100};
            var labelY = new Label {Location = new Point(0, labelX.Size.Height), Width = 100};
            var labelSpeed = new Label {Location = new Point(0, labelY.Location.Y + labelY.Size.Height), Width = 250};

            MouseClick += (sender, args) =>
            {
                if (coolDownShot <= 0)
                {
                    coolDownShot = localPlayer.CoolDown;
                    var vector = new Vector(args.Location.X - ClientSize.Width / 2,
                        args.Location.Y - ClientSize.Height / 2);
                    vector = vector / vector.Length * 10;
                    var b = new Bullet((int) vector.X, (int) vector.Y,
                        new Point((int) localPlayer.Position.X, (int) localPlayer.Position.Y), gameModel.Player.Damage);
                    gameModel.Shoot(b);
                }
            };

            var timer = new Timer {Interval = 16};
            timer.Tick += (sender, args) =>
            {
                labelX.Text = $"X: {localPlayer.Position.X}";
                labelY.Text = $"Y: {localPlayer.Position.Y}";
                labelSpeed.Text = $"Speed: {localPlayer.Speed}";

                // labelSpeed.Text = localPlayer.Speed.ToString();
                ReactOnControl(gameModel);
                gameModel.ChangePosition();
                coolDownShot -= 1;
                Refresh();
            };

            Controls.Add(labelY);
            Controls.Add(labelX);
            // Controls.Add(labelSpeed);
            InitializeComponent();
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            var graphic = args.Graphics;
            var carX = localPlayer.Position.X;
            var carY = localPlayer.Position.Y;
            var width = ClientSize.Width;
            var height = ClientSize.Height;
            graphic.DrawImage(grass, new Point(-(int) carX % 32 - 32, -(int) carY % 32 - 32));
            lock (gameModel.PlayerMap)
            {
                // graphic.TranslateTransform(width / 2, height / 2);
                // graphic.RotateTransform(
                //     (float) ((float) localPlayer.Direction / Math.PI * 180 + 90));
                // graphic.DrawImage(images[localPlayer.GetImage()], -17, -30);
                // graphic.ResetTransform();
                foreach (var player in gameModel.PlayerMap)
                {
                    graphic.TranslateTransform(NotBehindScreen(width / 2 - localPlayer.Position.X + player.Position.X),
                        NotBehindScreen(height / 2 - localPlayer.Position.Y + player.Position.Y));
                    graphic.FillEllipse(Brushes.Black, 0, 0, 5, 5);
                    graphic.RotateTransform((float) ((float) player.Direction / Math.PI * 180 + 90));
                    graphic.DrawImage(images[player.GetImage()], -17,-30);
                    graphic.ResetTransform();
                }
            }

            graphic.ResetTransform();
            graphic.TranslateTransform((float) -carX + width / 2, (float) -carY + height / 2);
            for (var x = ((int) carX - width / 2) / 32 - 2; x < ((int) carX + width / 2) / 32 + 1; x++)
            for (var y = ((int) carY - height / 2) / 32 - 2; y < ((int) carY + height / 2) / 32 + 1; y++)
            {
                var point = new Point(NotBehindScreen(x * 32), NotBehindScreen(y * 32));
                if (gameModel.Map.ContainsKey(point))
                {
                    graphic.DrawImage(images[gameModel.Map[point].GetImage()], x * 32, y * 32);
                    //graphic.DrawRectangle(Pens.Red, gameModel.Map[point].ObjRectangle);
                    if (gameModel.Map[point].Health != gameModel.Map[point].MaxHealth())
                    {
                        graphic.DrawRectangle(Pens.Black, x * 32, (y + 2) * 32, 32, 5);
                        graphic.FillRectangle(Brushes.GreenYellow, x * 32 + 1, (y + 2) * 32 + 1,
                            (float) 32 * ((float) gameModel.Map[point].Health / gameModel.Map[point].MaxHealth()), 4);
                    }
                }
            }

            lock (gameModel.Bullets)
            {
                foreach (var bullet in gameModel.Bullets)
                {
                    graphic.FillEllipse(Brushes.Black, bullet.Position.X, bullet.Position.Y, 5, 5);
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs args)
        {
            switch (args.KeyChar)
            {
                case '.':
                    localPlayer.Position = new Point(100, 100);
                    break;
                case 'W' or 'w' or 'ц' or 'Ц':
                    isWdown = true;
                    break;
                case 'S' or 's' or 'ы' or 'Ы':
                    isSdown = true;
                    break;
                case 'A' or 'a' or 'ф' or 'Ф':
                    isAdown = true;
                    break;
                case 'D' or 'd' or 'в' or 'В':
                    isDdown = true;
                    break;
                case 'F' or 'f' or 'а' or 'А':
                    if (WindowState == FormWindowState.Normal)
                        WindowState = FormWindowState.Maximized;
                    else
                        WindowState = FormWindowState.Normal;
                    break;
            }

            Activate();
        }

        protected override void OnKeyUp(KeyEventArgs args)
        {
            switch (args.KeyValue)
            {
                case 'W' or 'w' or 'ц' or 'Ц':
                    isWdown = false;
                    break;
                case 'S' or 's' or 'ы' or 'Ы':
                    isSdown = false;
                    break;
                case 'A' or 'a' or 'ф' or 'Ф':
                    isAdown = false;
                    break;
                case 'D' or 'd' or 'в' or 'В':
                    isDdown = false;
                    break;
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            var bmp = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Images", "grass.png"));
            grass = CreateColumn(CreatLine(bmp, ClientSize.Width + 64, ClientSize.Height + 64),
                ClientSize.Width + 64,
                ClientSize.Height + 64);
        }

        private int NotBehindScreen(int x) => x < 0 ? gameModel.Size + x :
            x >= gameModel.Size ? x - gameModel.Size : x;

        private Bitmap CreatLine(Image grass, int width, int height)
        {
            if (width < grass.Width) return (Bitmap) grass;
            var Image = CreatLine(grass, width / 2, height);
            var outputImage = new Bitmap(Image.Width * 2, grass.Height);
            Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image, new Point(0, 0));
            graphics.DrawImage(Image, new Point(Image.Width, 0));
            return outputImage;
        }

        private Bitmap CreateColumn(Image grass, int width, int height)
        {
            if (height < grass.Height) return (Bitmap) grass;
            var Image = CreateColumn(grass, width, height / 2);
            var outputImage = new Bitmap(width, Image.Height * 2);
            Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image, new Point(0, 0));
            graphics.DrawImage(Image, new Point(0, Image.Height));
            return outputImage;
        }


        private void ReactOnControl(GameModel game)
        {
            if (isWdown) game.Player.ChangeVelocity(KeyButton.Forward);
            if (isAdown) game.Player.ChangeDirection(KeyButton.Left);
            if (isDdown) game.Player.ChangeDirection(KeyButton.Right);
            if (isSdown) game.Player.ChangeVelocity(KeyButton.Backward);
            if (!isWdown && !isSdown)
                game.Player.ChangeVelocity(KeyButton.None);
        }
    }
}