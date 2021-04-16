using System.Windows.Forms;
using System.Drawing;

namespace Game
{
    public sealed partial class Map : Form
    {
        public Map(Game game)
        {
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
                    case 'w' or 'W':
                    {
                        game.Car.ChangeVelocity(KeyButton.Forward);    
                        break;
                    }
                    case 's' or 'S':
                    {
                        game.Car.ChangeVelocity(KeyButton.Backward);    
                        break;
                    }
                    case 'a' or 'A':
                    {
                        game.Car.ChangeDirection(KeyButton.Left);    
                        break;
                    }
                    case 'd' or 'D':
                    {
                        game.Car.ChangeDirection(KeyButton.Right);    
                        break;
                    }
                }
                game.ChangePlayerPosition();
            };
            
        }
    }
}