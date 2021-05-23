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
            var backButton = MakeBackButton();
            var list = GetAllResolutions(backButton);

            var buttonChangeResolution = ChooseResolution(list);
            
            Controls.Add(list);
            Controls.Add(backButton);
            Controls.Add(buttonChangeResolution);
            
            CloseOption(backButton);
            ChangeResolution(buttonChangeResolution, list);
        }

        private Button MakeBackButton()
        {
            var backButton = new Button
            {
                Location = new Point(ClientSize.Height / 4, ClientSize.Width / 4),
                Text = @"Back!",
                BackColor = Color.Chocolate,
            };
            return backButton;
        }

        private static ListBox GetAllResolutions(Button backButton)
        {
            var list = new ListBox
            {
                Location = new Point(backButton.Left, backButton.Bottom)
            };

            list.Items.AddRange(new object[]
            {
                new Tuple<int, int>(1280, 720),
                new Tuple<int, int>(1920, 1080),
                new Tuple<int, int>(2560, 1440)
            });
            return list;
        }

        private static Button ChooseResolution(ListBox list)
        {
            var buttonChangeResolution = new Button
            {
                Location = new Point(list.Left, list.Bottom),
                Text = @"ChangeResolution"
            };
            return buttonChangeResolution;
        }

        private void CloseOption(Button backButton)
        {
            backButton.Click += (_, __) =>
            {
                Program.Context.MainForm = new Menu();
                Close();
                Program.Context.MainForm.Show();
            };
        }

        private void ChangeResolution(Button buttonChangeResolution, ListBox list)
        {
            buttonChangeResolution.Click += (_, __) =>
            {
                var (width, height) = (Tuple<int, int>) list.SelectedItem;
                Program.screenSize = new Size(width, height);
                Program.Context.MainForm = new Options();
                Close();
                Program.Context.MainForm.Show();
            };
        }
    }
}