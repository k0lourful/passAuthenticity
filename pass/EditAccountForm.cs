using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace pass
{
    public partial class EditAccountForm : Form
    {
        private int ind;

        private List<Account> users;

        public EditAccountForm(List<Account> usersPar)
        {
            InitializeComponent();
            users = usersPar;
        }

        private void FillData()
        {
            textBox1.Text = users[ind].Login;
            checkBox1.Checked = users[ind].Banned;
            checkBox2.Checked = users[ind].PassLimited;
        }

        private void EditAccountForm_Load(object sender, EventArgs e)
        {
            FillData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ++ind;
            if (ind > users.Count - 1)
                ind = 0;
            FillData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            users[ind].Login = textBox1.Text;
            users[ind].Banned = checkBox1.Checked;
            users[ind].PassLimited = checkBox2.Checked;
        }
    }
}
