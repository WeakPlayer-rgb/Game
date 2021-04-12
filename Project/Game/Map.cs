using System.Windows.Forms;
using System.Drawing;

namespace Game
{
    public sealed partial class Map : Form
    {
        public Map()
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

            var map = new int[10000, 10000];
            
        }
    }
}