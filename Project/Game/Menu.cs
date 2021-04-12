using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            BackColor = Color.Black;
            var startButton = new Button
            {
                Location = new Point(ClientSize.Width / 2, ClientSize.Height / 2),
                Size = new Size(100, 50),
                Text = @"Start!",
                BackColor = Color.Azure
            };
            var optionButton = new Button
            {
                Location = new Point(startButton.Left, startButton.Bottom),
                Size = new Size(100, 50),
                Text = @"Options",
                BackColor = Color.Chartreuse
            };

            Controls.Add(startButton);
            Controls.Add(optionButton);
            startButton.Click += (_, _) =>
            {
                Program.Context.MainForm = new Map();
                Close();
                Program.Context.MainForm.Show();
            };
            optionButton.Click += (_, _) =>
            {
                Program.Context.MainForm = new Options();
                Close();
                Program.Context.MainForm.Show();
            };
        }

        public sealed override Color BackColor { get; set; }
    }
}