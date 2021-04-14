using System.Windows.Forms;

namespace pass
{
    public partial class ChangePasswordForm : Form
    {
        public string oldPass;

        public string newPass;

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void ChangePasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                if(textBox2.Text == textBox3.Text)
                {
                    newPass = textBox2.Text;
                    oldPass = textBox1.Text;
                }
                else
                {
                    e.Cancel = true;
                    MessageBox.Show("Пароли не совпадают!", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }
    }
}
