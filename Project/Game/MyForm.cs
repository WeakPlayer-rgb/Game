using System.Windows.Forms;

namespace Game
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
            
            FormClosing += (sender,eventArgs) =>{
                    var result = MessageBox.Show("Действительно закрыть?", "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result != DialogResult.Yes)
                        eventArgs.Cancel = true;
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs eventArgs)
        {
            MessageBox.Show("R u sure u wanna exit?", "DONT DO IT", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            eventArgs.Cancel = true;
        }
    }
}