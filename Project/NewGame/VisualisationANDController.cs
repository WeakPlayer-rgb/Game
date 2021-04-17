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
    public sealed partial class VisualisationANDController : Form
    {
        private GameModel _gameModel;
        private Physics standardPhysics;
        public Bitmap Car = image.car;
        private Label label;
        public VisualisationANDController(GameModel g)
        {
            _gameModel = g;
            KeyPreview = true;
            standardPhysics = new Physics();
            DoubleBuffered = true;
            label = new Label{Location = new Point(500,500), MaximumSize = ClientSize};
            /*var button = new Button
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
            };*/
            
            KeyPress += (sender, args) =>
            {
                switch (args.KeyChar)
                {
                    case '.':
                        _gameModel.Car.Position = new Vector(1000, 1000);
                        _gameModel.Car.Direction = new Vector(-10,-5);
                        break;
                    case 'W':
                    case 'w':
                    {
                        _gameModel.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 'S':
                    case 's':
                    {
                        _gameModel.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'A':
                    case 'a':
                    {
                        _gameModel.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'D':
                    case 'd':
                    {
                        _gameModel.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }

                //Physics.MoveCar(new Car(new Vector(2, 3), Vector.Zero, 3, 1, 2), 3, Turn.Left, 3);
                label.Text = string.Format($@"{args.KeyChar}");
                Refresh();
            };
            
            var timer = new Timer {Interval = 50};
            timer.Tick += (sender, args) =>
            {
                _gameModel.Car.ChangeVelocity(KeyButton.None);
                _gameModel.ChangePosition();
                Activate();
                Refresh();
            };
            Paint += (sender, args) =>
            {
                var graphic = args.Graphics;
                graphic.ScaleTransform(0.1f,0.1f);
                graphic.FillEllipse(Brushes.Blue, new Rectangle(new Point((int)_gameModel.Car.Position.X,(int)_gameModel.Car.Position.Y), new Size(30, 30)));
                graphic.TranslateTransform((int)_gameModel.Car.Position.X-67,(int)_gameModel.Car.Position.Y-32 );
                graphic.RotateTransform((float) ((float)_gameModel.Car.Direction.Angle/Math.PI*180+180) /*((int)_gameModel.Car.Direction.Angle/2/Math.PI*360*/);
                graphic.DrawImage(Car,(int)_gameModel.Car.Position.X,(int)_gameModel.Car.Position.Y);
                graphic.TranslateTransform((int)-_gameModel.Car.Position.X+67,(int)-_gameModel.Car.Position.Y+32);
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