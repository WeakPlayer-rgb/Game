using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms.PropertyGridInternal;
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
        private readonly Bitmap car = image.test;
        private bool isWdown = false;
        private bool isAdown = false;
        private bool isDdown = false;
        private bool isSdown = false;

        public VisualisationAndController(GameModel g)
        {
            Activate();
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

            var timer = new Timer { Interval = 100 };
            timer.Tick += (sender, args) =>
            {
                ReactOnControl(gameModel);
                gameModel.ChangePosition();
                Refresh();
            };
            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                for (int x = 0; x < ClientSize.Width; x += 32)
                    for (int y = 0; y < ClientSize.Height; y += 32)
                        graphic.DrawImage(image.grass, new Point(x, y));
                graphic.TranslateTransform((int)gameModel.Car.Position.X, (int)gameModel.Car.Position.Y);
                graphic.RotateTransform(
                    (float)((float)gameModel.Car.Direction.Angle / Math.PI * 180 + 90) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphic.DrawImage(gameModel.Car.GetImage(), -14, -25);
                graphic.TranslateTransform(-(int)gameModel.Car.Position.X, -(int)gameModel.Car.Position.Y);


            };
            InitializeComponent();
            timer.Start();
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