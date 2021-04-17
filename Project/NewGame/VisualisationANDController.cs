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
        public Bitmap Car = image.car1;
        private Label label;
        public VisualisationAndController(GameModel g)
        {
            gameModel = g;
            KeyPreview = true;
            standardPhysics = new Physics();
            DoubleBuffered = true;
            label = new Label{Location = new Point(500,500), MaximumSize = ClientSize};
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
                switch (args.KeyChar)
                {
                    case 'W':
                    case 'w':
                    {
                        gameModel.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 'S':
                    case 's':
                    {
                        gameModel.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'A':
                    case 'a':
                    {
                        gameModel.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'D':
                    case 'd':
                    {
                        gameModel.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }

                //Physics.MoveCar(new Car(new Vector(2, 3), Vector.Zero, 3, 1, 2), 3, Turn.Left, 3);
                label.Text += string.Format($@"{args.KeyChar}");
                Refresh();
            };
            
            var timer = new Timer {Interval = 10};
            timer.Tick += (sender, args) =>
            {
                Activate();
                Refresh();
            };
            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                graphic.FillEllipse(Brushes.Aquamarine, 
                    new Rectangle((int)gameModel.Car.Position.X,(int)gameModel.Car.Position.Y,3,3));
                graphic.RotateTransform((float) ((int)gameModel.Car.Direction.Angle/2/Math.PI*360));
                graphic.DrawImage(Car,(int)gameModel.Car.Position.X,(int)gameModel.Car.Position.Y);
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