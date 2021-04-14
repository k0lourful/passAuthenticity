using System.Windows.Forms;

namespace pass
{
    public partial class NewPasswordForm : Form
    {
        public string newPass;

        public NewPasswordForm()
        {
            InitializeComponent();
        }

        private void NewPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (textBox1.Text != textBox2.Text)
                {
                    MessageBox.Show("Пароли не совпадают!", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    e.Cancel = true;
                }
                else
                {
                    newPass = textBox2.Text;
                }
            }
        }
    }
}
