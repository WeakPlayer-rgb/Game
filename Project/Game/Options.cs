using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Game
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var backButton = new Button
            {
                Location = new Point(ClientSize.Height / 3, ClientSize.Width / 3),
                Text = @"Back!"
            };
            var list = new ListBox
            {
                Location = new Point(backButton.Left,backButton.Top)
            };
            
            list.Items.AddRange(new []{ "1280*720","1920*1080","2560*1440"});
            Controls.Add(list);
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