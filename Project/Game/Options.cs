using System;
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
            
            list.Items.AddRange(new object[]
            {
                new Tuple<int,int>(1280, 720),
                new Tuple<int,int>(1920, 1080),
                new Tuple<int,int>(2560, 1440)
            });

            var buttonChangeResolution = new Button
            {
                Location = new Point(list.Left,list.Bottom),
                Text = @"ChangeResolution"
            };
            
            Controls.Add(list);
            Controls.Add(backButton);
            Controls.Add(buttonChangeResolution);
            backButton.Click += (_, _) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
            buttonChangeResolution.Click += (s, x) =>
            {
                var (width, height) = (Tuple<int,int>)list.SelectedItem;
                Program.screenSize = new Size(width, height);
            };
        }
    }
}