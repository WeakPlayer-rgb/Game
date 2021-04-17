using System;
using System.Drawing;
using System.Windows.Forms;

namespace NewGame
{
    public sealed partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var backButton = new Button
            {
                Location = new Point(ClientSize.Height / 4, ClientSize.Width / 4),
                Text = @"Back!",
                BackColor = Color.Chocolate,
            };
            var list = new ListBox
            {
                Location = new Point(backButton.Left,backButton.Bottom)
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
            backButton.Click += (_, __) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
            buttonChangeResolution.Click += (_, __) =>
            {
                
                var (width, height) = (Tuple<int,int>)list.SelectedItem;
                Program.screenSize = new Size(width, height);
                Program.Context.MainForm = new Options();
                Close();
                Program.Context.MainForm.Show();
            };
        }
    }
}