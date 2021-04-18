using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;

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

        public VisualisationAndController(GameModel g)
        {
            gameModel = g;
            KeyPreview = true;
            standardPhysics = new Physics();
            DoubleBuffered = true;
            var label = new Label{Location = new Point(500,500), MaximumSize = ClientSize};
            // var button = new Button
            // {
            //     Location = new Point(0,0),
            //     Text = @"Back!"
            // };
            // Controls.Add(button);
            // button.Click += (_, _) =>
            // {
            //     Program.Context.MainForm = new Menu();
            //     Close();
            //     Program.Context.MainForm.Show();
            // };
            
            KeyPress += (sender, args) =>
            {
                if (args.KeyChar == '.')
                {
                    gameModel.Car.Position = new Vector(500, 500);
                    gameModel.Car.Direction = new Vector(-10, -5);
                }
                if (args.KeyChar is 'W' or 'w')
                {
                    gameModel.Car.ChangeVelocity(KeyButton.Forward);
                }
                else if (args.KeyChar is 'S' or 's')
                {
                    gameModel.Car.ChangeVelocity(KeyButton.Backward);
                }
                if (args.KeyChar is 'A' or 'a')
                {
                    gameModel.Car.ChangeDirection(KeyButton.Left);
                }
                else if (args.KeyChar is 'D' or 'd')
                {
                    gameModel.Car.ChangeDirection(KeyButton.Right);
                }

                //Physics.MoveCar(new Car(new Vector(2, 3), Vector.Zero, 3, 1, 2), 3, Turn.Left, 3);
                label.Text = string.Format($@"{args.KeyChar}");
                Refresh();
            };
            
            var timer = new Timer {Interval = 50};
            timer.Tick += (sender, args) =>
            {
                gameModel.Car.ChangeVelocity(KeyButton.None);
                gameModel.ChangePosition();
                Activate();
                Refresh();
            };
            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                graphic.TranslateTransform((int)gameModel.Car.Position.X,(int)gameModel.Car.Position.Y);
                graphic.RotateTransform((float) ((float)gameModel.Car.Direction.Angle/Math.PI*180+90) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphic.DrawImage(car,-14,-25);
                graphic.TranslateTransform(-(int)gameModel.Car.Position.X,-(int)gameModel.Car.Position.Y);
                
                // for (var y =(int) game.Car.Position.Y - ClientSize.Height / 2;
                //     y < game.Car.Position.Y + ClientSize.Height / 2;
                //     y++)
                // {
                //     for (var x =(int) game.Car.Position.X - ClientSize.Width / 2;
                //         x < game.Car.Position.X + ClientSize.Width / 2;
                //         x++)
                //     {
                //         if (game.Map[y, x] != null)
                //         {
                //             
                //         }
                //     }
                // }
            };
            InitializeComponent();
            Controls.Add(label);
            timer.Start();
        }
    }
}