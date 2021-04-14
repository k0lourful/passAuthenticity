using System.Windows.Forms;

namespace pass
{
    public partial class AddAccountForm : Form
    {
        public string userName;

        public bool userBlocked;

        public bool passLimited;

        public AddAccountForm()
        {
            InitializeComponent();
        }

        private void AddAccountForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                userName = textBox1.Text;
                userBlocked = checkBox1.Checked;
                passLimited = checkBox2.Checked;
            }
        }
    }
}
