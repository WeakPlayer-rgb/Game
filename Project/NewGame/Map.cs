﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;

namespace NewGame
{
    public sealed partial class Map : Form
    {
        private Game game;
        public Bitmap Car = image.car;
        private Label label;
        public Map(Game g)
        {
            KeyPreview = true;
            game = g;
            InitializeComponent();
            DoubleBuffered = true;
            label = new Label{Location = new Point(500,500)};
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
                    case 'w':
                    {
                        game.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 's':
                    {
                        game.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'a':
                    {
                        game.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'd':
                    {
                        game.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }
                game.ChangePlayerPosition();
                label.Text += "Xyu";
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
            Controls.Add(label);
            timer.Start();
        }
    }
}