﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
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
    /// 
    /// </summary>
    public sealed partial class VisualisationAndController : Form
    {
        public Graphics[] GameBorders = new Graphics[] { };
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
                        gameModel.Car.Position = new Vector(10, 10);
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

            var timer = new Timer { Interval = 5 };
            timer.Tick += (sender, args) =>
            {
                labelX.Text = "X: " + gameModel.Car.Position.X.ToString();
                labelY.Text = "Y: " + gameModel.Car.Position.Y.ToString();
                ReactOnControl(gameModel);
                gameModel.ChangePosition();
                Refresh();
            };


            Paint += (sender, args) =>
            {
                var graphics = args.Graphics;
                var carX = gameModel.Car.Position.X;
                var carY = gameModel.Car.Position.Y;
                var width = ClientSize.Width;
                var height = ClientSize.Height;
                var tree = new Tree().GetImage();
                graphics.DrawImage(grass, new Point(-(int)carX % 32 - 32, -(int)carY % 32 - 32));

                graphics.TranslateTransform(width / 2, height / 2);
                graphics.RotateTransform(
                    (float)((float)gameModel.Car.Direction / Math.PI * 180 + 90) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphics.DrawImage(gameModel.Car.GetImage(), -17, -30);
                //graphic.FillEllipse(Brushes.Black, 0, 0, 2, 2);
                graphics.TranslateTransform(-width / 2, -height / 2);
                graphics.ResetTransform();
                graphics.TranslateTransform((float)-carX + width / 2, (float)-carY + height / 2);
                foreach (var obj in gameModel.Map.Keys)
                {
                    if (carX - width / 1.5 < obj.X*32 && obj.X*32 < carX + width / 1.5 && 
                        carY - height / 1.5 < obj.Y*32 && obj.Y*32 < carY + height / 1.5)
                        graphics.DrawImage(tree, obj.X * 32, obj.Y * 32);
                }
                graphics.FillEllipse(Brushes.Red, -5, -5, 10, 10);
                MakeGameBorders(graphics);
            };
            Controls.Add(labelY);
            Controls.Add(labelX);
            InitializeComponent();
            timer.Start();
        }

        private static void MakeGameBorders(Graphics graphic)
        {
            graphic.FillRectangle(Brushes.Red, 0, 0, 10000, 5);
            graphic.FillRectangle(Brushes.Red, 0, 0, 5, 10000);
            graphic.FillRectangle(Brushes.Red, 9995, 0, 5, 10000);
            graphic.FillRectangle(Brushes.Red, 0, 9995, 10000, 5);
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
            if (isWdown) game.Car.ChangeVelocity(KeyButton.Forward);
            if (isAdown) game.Car.ChangeDirection(KeyButton.Left);
            if (isDdown) game.Car.ChangeDirection(KeyButton.Right);
            if (isSdown) game.Car.ChangeVelocity(KeyButton.Backward);
            if (!isWdown && !isSdown)
                game.Car.ChangeVelocity(KeyButton.None);
        }
    }
}