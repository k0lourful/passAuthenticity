using System;
using System.Windows.Forms;

namespace pass
{
    public partial class LoginForm : Form
    {
        public string name;

        public string pass;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            pass = textBox2.Text;
        }
    }
}
