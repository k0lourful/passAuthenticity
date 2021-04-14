using System.Collections.Generic;
using System.Windows.Forms;

namespace pass
{
    public partial class ListAccountsForm : Form
    {
        public ListAccountsForm(List<Account> users)
        {
            InitializeComponent();
            foreach (Account user in users)
                dataGridView1.Rows.Add(user.Login, user.Banned, user.PassLimited);
        }
    }
}
