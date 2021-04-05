using System.Windows.Forms;

namespace Game
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs eventArgs)
        {
            MessageBox.Show("R u sure u wanna exit?", "DONT DO IT", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            eventArgs.Cancel = true;
        }
    }
}