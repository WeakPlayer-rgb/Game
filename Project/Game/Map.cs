using System.Windows.Forms;
using System.Drawing;

namespace Game
{
    public partial class Map : Form
    {
        public Map()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var button = new Button
            {
                Location = new Point(ClientSize.Height/3,ClientSize.Width/3),
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