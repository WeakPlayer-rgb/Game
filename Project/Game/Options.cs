using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class Options : Form
    {
        public Options()
        {
            DoubleBuffered = true;
            var backButton = new Button
            {
                Location = new Point(ClientSize.Height / 3, ClientSize.Width / 3),
                Text = "Back!"
            };
            Controls.Add(backButton);
            backButton.Click += (_, _) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
        }
    }
}