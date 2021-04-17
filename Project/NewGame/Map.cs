using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;
// using Game;

namespace NewGame
{
    public sealed partial class Map : Form
    {
        private Game game;
        public Bitmap Car = image.car;
        public Map(Game g)
        {
            game = g;
            InitializeComponent();
            DoubleBuffered = true;
            
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
                    {
                        game.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 'S':
                    {
                        game.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'A':
                    {
                        game.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'D':
                    {
                        game.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }
                game.ChangePlayerPosition();
                Console.WriteLine(game.Car.Direction);
                Refresh();
            };
            
            var timer = new Timer {Interval = 100};
            timer.Tick += (sender, args) => Refresh();

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
            timer.Start();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'W':
                {
                    game.Car.ChangeVelocity(KeyButton.Forward);    
                    break;
                }
                case 'S':
                {
                    game.Car.ChangeVelocity(KeyButton.Backward);    
                    break;
                }
                case 'A':
                {
                    game.Car.ChangeDirection(KeyButton.Left);    
                    break;
                }
                case 'D':
                {
                    game.Car.ChangeDirection(KeyButton.Right);    
                    break;
                }
            }
            game.ChangePlayerPosition();
            Console.WriteLine(game.Car.Direction);
        }
    }
}