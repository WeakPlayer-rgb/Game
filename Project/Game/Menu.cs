using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Game
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            var startButton = new Button
            {
                Location = new Point(ClientSize.Width / 2, ClientSize.Height / 2),
                Size = new Size(100, 50),
                Text = "Start!"
            };
            var optionButton = new Button
            {
                Location = new Point(startButton.Left, startButton.Bottom),
                Size = new Size(100, 50),
                Text = "Options"
            };

            Controls.Add(startButton);
            Controls.Add(optionButton);
            startButton.Click += (s, e) =>
            {
                Program.Context.MainForm = new Map();
                Close();
                Program.Context.MainForm.Show();
            };
            optionButton.Click += (s, e) =>
            {
                Program.Context.MainForm = new Options();
                Close();
                Program.Context.MainForm.Show();
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Действительно закрыть?", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}