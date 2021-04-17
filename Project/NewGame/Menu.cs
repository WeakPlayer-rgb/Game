using System.Drawing;
using System.Windows.Forms;

namespace NewGame
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
            var exitButton = new Button()
            {
                Location = new Point(optionButton.Left, optionButton.Bottom),
                Size = new Size(100, 50),
                Text = "Exit",
                BackColor = Color.Aqua,
            };

            Controls.Add(startButton);
            Controls.Add(optionButton);
            Controls.Add(exitButton);
            startButton.Click += (s, e) =>
            {
                Program.Context.MainForm = new Map(new Game(10000));
                Close();
                Program.Context.MainForm.Show();
            };
            optionButton.Click += (s, e) =>
            {
                Program.Context.MainForm = new Options();
                Close();
                Program.Context.MainForm.Show();
            };
            exitButton.Click += (s, e) =>
            {
                var result = MessageBox.Show(@"Действительно закрыть?", "Не надо этого делать...",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) Close();
            };
        }
        public override Color BackColor { get; set; }
    }
}