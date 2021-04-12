using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            var button = new Button
            {
                Location = new Point(ClientSize.Width/2, ClientSize.Height/2),
                Size = new Size(100,50),
                Text = "Start!"
            };
            
            Controls.Add(button);
            button.Click += (_, _) =>
            {
                Program.Context.MainForm = new Map();
                Close();
                Program.Context.MainForm.Show();
            };
        }
    }
}