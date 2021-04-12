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
                Location = new Point(0,ClientSize.Height),
                Text = @"Back!"
            };
            Controls.Add(button);
            button.Click += (_, _) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
        }
    }
}