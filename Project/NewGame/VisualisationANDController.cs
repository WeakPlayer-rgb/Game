using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
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
        private Physics standardPhysics;
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
            standardPhysics = new Physics();
            DoubleBuffered = true;
            var label = new Label { Location = new Point(500, 500), MaximumSize = ClientSize };
            var queue = new Queue<int>();
            //TODO: keyPress is just one repeatable action. KeyDown needed.
            KeyPress += (sender, args) =>
            {
                switch (args.KeyChar)
                {
                    case '.':
                        gameModel.Car.Position = new Vector(500, 500);
                        break;
                    case 'W' or 'w':
                        isWdown = true;
                        break;
                    case 'S' or 's':
                        isSdown = true;
                        break;
                    case 'A' or 'a':
                        isAdown = true;
                        break;
                    case 'D' or 'd':
                        isDdown = true;
                        break;
                }
                Activate();
                Refresh();
            };
            KeyUp += (sender, args) =>
            {
                switch (args.KeyValue)
                {
                    case 'W' or 'w':
                        isWdown = false;
                        break;
                    case 'D' or 'd':
                        isDdown = false;
                        break;
                    case 'A' or 'a':
                        isAdown = false;
                        break;
                    case 'S' or 's':
                        isSdown = false;
                        break;
                }
            };

            var timer = new Timer { Interval = 5 };
            timer.Tick += (sender, args) =>
            {
                ReactOnControl(gameModel);
                gameModel.ChangePosition();
                Refresh();
            };
            

            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                
                graphic.DrawImage(grass, new Point(0, 0));
                graphic.TranslateTransform((int)gameModel.Car.Position.X, (int)gameModel.Car.Position.Y);
                graphic.RotateTransform(
                    (float)((float)gameModel.Car.Direction.Angle / Math.PI * 180 + 90) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphic.DrawImage(gameModel.Car.GetImage(), -14, -25);
                graphic.TranslateTransform(-(int)gameModel.Car.Position.X, -(int)gameModel.Car.Position.Y);


            };
            InitializeComponent();
            timer.Start();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            var PathToGrass = Path.Combine(Directory.GetCurrentDirectory(), "Images", "grass.png");
            var bmp = Image.FromFile(PathToGrass);
            grass = CreateColumn(CreatLine(bmp, ClientSize.Width, ClientSize.Height), ClientSize.Width, ClientSize.Height);
        }

        private Bitmap CreatLine(Image grass, int width, int height)
        {
            if (width < grass.Width) return (Bitmap)grass;
            var Image = CreatLine(grass, width / 2, height);
            var outputImage = new Bitmap(Image.Width * 2, grass.Height);
            using Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image, new Point(0, 0));
            graphics.DrawImage(Image,new Point(Image.Width,0));
            return outputImage;
        }

        private Bitmap CreateColumn(Image grass, int width, int height)
        {
            if (height < grass.Height) return (Bitmap) grass;
            var Image = CreateColumn(grass, width, height / 2);
            var outputImage = new Bitmap(width, Image.Height * 2);
            using Graphics graphics = Graphics.FromImage(outputImage);
            graphics.DrawImage(Image,new Point(0,0));
            graphics.DrawImage(Image,new Point(0,Image.Height));
            return outputImage;
        }

        void ReactOnControl(GameModel game)
        {
            if (isWdown) game.Car.ChangeVelocity(KeyButton.Forward);
            if (isAdown) game.Car.ChangeDirection(KeyButton.Left);
            if (isDdown) game.Car.ChangeDirection(KeyButton.Right);
            if (isSdown) game.Car.ChangeVelocity(KeyButton.Backward);
            if (!isWdown && !isSdown)
                game.Car.ChangeVelocity(KeyButton.None);
        }
    }
}