using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;

namespace NewGame
{
    public sealed partial class GameVisualisation : Form
    {
        private Game game;
        private Physics standardPhysics;
        public Bitmap Car = image.car;
        private Label label;
        public GameVisualisation(Game g)
        {
            game = g;
            KeyPreview = true;
            standardPhysics = new Physics();
            DoubleBuffered = true;
            label = new Label{Location = new Point(500,500), AutoSize = true};
            var button = new Button
            {
                Location = new Point(0,0),
                Text = @"Back!"
            };
            Controls.Add(button);
            button.Click += (_, _) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
            
            KeyPress += (sender, args) =>
            {
                switch (args.KeyChar)
                {
                    case 'W':
                    case 'w':
                    {
                        game.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 'S':
                    case 's':
                    {
                        game.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'A':
                    case 'a':
                    {
                        game.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'D':
                    case 'd':
                    {
                        game.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }

                //Physics.MoveCar(new Car(new Vector(2, 3), Vector.Zero, 3, 1, 2), 3, Turn.Left, 3);
                label.Text += string.Format($@"{args.KeyChar}");
                Refresh();
            };
            
            var timer = new Timer {Interval = 100};
            timer.Tick += (sender, args) =>
            {
                Activate();
                Refresh();
            };
            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                graphic.ScaleTransform(0.1f,0.1f);
                graphic.RotateTransform((float) ((int)game.Car.Direction.Angle/2/Math.PI*360));
                graphic.DrawImage(Car,ClientSize.Height/2,ClientSize.Width/2);
                graphic.ResetTransform();
                
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