using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.PropertyGridInternal;
using static System.Drawing.Bitmap;

// ReSharper disable All

namespace NewGame
{
    /// <summary>
    /// TODO: Split visual and control part apart. Doesn't fit in MVC. Ask Ilya if that's ok.
    /// </summary>
    public sealed partial class VisualisationAndController : Form
    {
        private GameModel gameModel;
        private bool isWdown = false;
        private bool isAdown = false;
        private bool isDdown = false;
        private bool isSdown = false;
        private Image grass;
        public VisualisationAndController(GameModel g)
        {
            Activate();
            var PathToGrass = Path.Combine(Directory.GetCurrentDirectory(), "Images", "grass.png");
            var bmp = Image.FromFile(PathToGrass);
            var Grass = CreateColumn(CreatLine(bmp, ClientSize.Width, ClientSize.Height), ClientSize.Width, ClientSize.Height);
            gameModel = g;
            KeyPreview = true;
            DoubleBuffered = true;
            var labelX = new Label { Location = new Point(0, 0), Width = 150 };
            var labelY = new Label { Location = new Point(0, labelX.Size.Height), Width = 150 };
            KeyPress += (sender, args) =>
            {
                switch (args.KeyChar)
                {
                    case '.':
                        gameModel.Player.Position = new Vector(10, 10);
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
                Refresh();
            };
            KeyUp += (sender, args) =>
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
            };

            var timer = new Timer { Interval = 20 };
            timer.Tick += (sender, args) =>
            {
                labelX.Text = "X: " + gameModel.Player.Position.X.ToString();
                labelY.Text = "Y: " + gameModel.Player.Position.Y.ToString();
                ReactOnControl(gameModel);
                gameModel.ChangePosition();
                Refresh();
            };


            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                var carX = gameModel.Player.Position.X;
                var carY = gameModel.Player.Position.Y;
                var width = ClientSize.Width;
                var height = ClientSize.Height;
                var tree = new Tree(Point.Empty).GetImage();
                graphic.DrawImage(grass, new Point(-(int)carX % 32 - 32, -(int)carY % 32 - 32));

                graphic.TranslateTransform(width / 2, height / 2);
                graphic.RotateTransform(
                    (float)((float)gameModel.Player.Direction / Math.PI * 180 + 90) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphic.DrawImage(gameModel.Player.GetImage(), -17, -30);
                graphic.DrawRectangle(Pens.Red, -17,-30,35,65);
                //graphic.FillEllipse(Brushes.Black, 0, 0, 2, 2);
                graphic.TranslateTransform(-width / 2, -height / 2);
                graphic.ResetTransform();
                graphic.TranslateTransform((float)-carX + width / 2, (float)-carY + height / 2);
                for (var x = ((int)carX - width / 2) / 32 - 2; x < ((int)carX + width / 2) / 32 + 1; x++)
                    for (var y = ((int)carY - height / 2) / 32 - 2; y < ((int)carY + height / 2) / 32 + 1; y++)
                    {
                        var point = new Point(NotBehindScreen(x * 32), NotBehindScreen(y * 32));
                        if (gameModel.Map.ContainsKey(point))
                        {
                            graphic.DrawImage(tree, x * 32, y * 32);
                            graphic.DrawRectangle(Pens.Red, x*32,y*32,40,75);
                        }
                    }
                //graphic.FillEllipse(Brushes.Red, -5, -5, 10, 10);
                //graphic.FillRectangle(Brushes.Red, 0, 0, 10000, 5);
                //graphic.FillRectangle(Brushes.Red, 0, 0, 5, 10000);
                //graphic.FillRectangle(Brushes.Red, 9995, 0, 5, 10000);
                //graphic.FillRectangle(Brushes.Red, 0, 9995, 10000, 5);
            };
            Controls.Add(labelY);
            Controls.Add(labelX);
            InitializeComponent();
            timer.Start();
        }

        private int NotBehindScreen(int x)
        {
            return x < 0 ? gameModel.size + x : x >= gameModel.size ? x - gameModel.size : x;
        }


        protected override void OnClientSizeChanged(EventArgs e)
        {
            var PathToGrass = Path.Combine(Directory.GetCurrentDirectory(), "Images", "grass.png");
            var bmp = Image.FromFile(PathToGrass);
            grass = CreateColumn(CreatLine(bmp, ClientSize.Width + 64, ClientSize.Height + 64), ClientSize.Width + 64, ClientSize.Height + 64);
        }

        private Bitmap CreatLine(Image grass, int width, int height)
        {
            if (width < grass.Width) return (Bitmap)grass;
            var Image = CreatLine(grass, width / 2, height);
            var outputImage = new Bitmap(Image.Width * 2, grass.Height);
            Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image, new Point(0, 0));
            graphics.DrawImage(Image, new Point(Image.Width, 0));
            return outputImage;
        }

        private Bitmap CreateColumn(Image grass, int width, int height)
        {
            if (height < grass.Height) return (Bitmap)grass;
            var Image = CreateColumn(grass, width, height / 2);
            var outputImage = new Bitmap(width, Image.Height * 2);
            Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image, new Point(0, 0));
            graphics.DrawImage(Image, new Point(0, Image.Height));
            return outputImage;
        }

        void ReactOnControl(GameModel game)
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