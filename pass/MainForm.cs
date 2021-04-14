using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace pass
{
    public partial class MainForm : Form
    {
        private const string fileName = "accounts.txt";

		private const string auditFileName = "audit.txt";

        private Account user;

        private List<Account> users = new List<Account>();

		System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        public MainForm()
        {
            InitializeComponent();
        }

        private void оПроектеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void списокПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListAccountsForm listAccountsForm = new ListAccountsForm(users);
            listAccountsForm.ShowDialog();
        }

        private void редактироватьПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAccountForm editAccountForm = new EditAccountForm(users);
            editAccountForm.ShowDialog();
        }

        private void добавитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (true)
            {
                bool loginFree = true;
                AddAccountForm addUserForm = new AddAccountForm();
                if (addUserForm.ShowDialog() != DialogResult.OK)
                {
                    break;
                }
                foreach (Account user2 in users)
                {
                    if (user2.Login == addUserForm.userName)
                    {
                        MessageBox.Show("Пользователь " + addUserForm.userName + " уже существует.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        loginFree = false;
                        break;
                    }
                }
                if (loginFree)
                {
                    users.Add(new Account(addUserForm.userName, "", addUserForm.userBlocked, addUserForm.passLimited));
                    break;
                }
            }
        }

        private void изменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm();
			while (true)
			{
				if (changePasswordForm.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				if (user.Pass == changePasswordForm.oldPass)
				{
					if (user.PassLimited)
					{
						if (Account.IsPasswordValid(changePasswordForm.newPass))
						{
							user.Pass = changePasswordForm.newPass;
							break;
						}
						else
						{
							MessageBox.Show("Пароль не соответствует ограничениям.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					else
					{
						user.Pass = changePasswordForm.newPass;
						break;
					}
				}
				else
				{
					MessageBox.Show("Неверный старый пароль.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
			watch.Start();
			BinaryReader binaryReader = null;

			if (File.Exists(fileName))
			{
				try
				{
					binaryReader = new BinaryReader(File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read));
					while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
					{
						users.Add(new Account(binaryReader.ReadString(), binaryReader.ReadString(), binaryReader.ReadBoolean(), binaryReader.ReadBoolean()));
					}
					binaryReader.Close();
				}
				catch (Exception)
				{
					MessageBox.Show("Ошибка при чтении файла.");
					binaryReader?.Close();
					Close();
					return;
				}
			}
			else
			{
				users.Add(new Account("admin", "", false, false));
			}

			StreamWriter auditWriter = null;
			try
			{
				auditWriter = new StreamWriter(File.Open(auditFileName, FileMode.Append, FileAccess.Write));
			}
			catch
			{
				MessageBox.Show("Ошибка при чтении файла аудита.");
				auditWriter?.Close();
				Close();
				return;
			}

			user = null;
			LoginForm loginForm = new LoginForm();
			int num = 0;
			bool loginMatched;
			bool passwordMatched;

			while (true)
			{
				if (loginForm.ShowDialog() == DialogResult.OK)
				{
					loginMatched = false;
					passwordMatched = false;
					foreach (Account u in users)
					{
						user = u;
						if (user.Login == loginForm.name && (user.Pass == loginForm.pass || user.Pass == ""))
						{
							loginMatched = true;
							passwordMatched = true;
							auditWriter.Write("Успех: " + user.Login + " -- Введённый пароль: " + loginForm.pass + " -- Время действия: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " -- Длительность сеанса: " + watch.Elapsed.Minutes + "m:" + watch.Elapsed.Seconds + "s:" + watch.Elapsed.Milliseconds + "ms\n");
							break;
						}
						if (user.Login == loginForm.name && user.Banned)
						{
							loginMatched = true;
							break;
						}
						if (user.Login == loginForm.name && user.Pass != loginForm.pass)
						{
							loginMatched = true;
							if (++num == 3)
							{
								MessageBox.Show("Вы трёхкратно ввели неверный пароль, работа будет завершена.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							}
							else
							{
								MessageBox.Show("Неверный пароль.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							}
							auditWriter.Write("Неудача: " + user.Login + " -- Введённый пароль: " + loginForm.pass + " -- Время действия: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " -- Длительность сеанса: " + watch.Elapsed.Minutes + "m:" + watch.Elapsed.Seconds + "s:" + watch.Elapsed.Milliseconds + "ms\n");
							break;
						}
					}
					if (loginMatched && user.Banned)
					{
						MessageBox.Show("Пользователь " + user.Login + " заблокирован.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						continue;
					}
					if (!loginMatched)
					{
						MessageBox.Show("Пользователь " + loginForm.name + " не найден.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						num = 0;
						continue;
					}
					if (loginMatched && num == 3)
					{
						break;
					}
					if (!loginMatched || !passwordMatched)
					{
						continue;
					}
					toolStripStatusLabel2.Text = loginForm.name;
					if (loginForm.name != "admin")
					{
						списокПользователейToolStripMenuItem.Visible = false;
						добавитьПользователяToolStripMenuItem.Visible = false;
						редактироватьПользователейToolStripMenuItem.Visible = false;
					}
					break;
				}
				Close();
				return;
			}
			auditWriter.Close();

			if (!loginMatched || !passwordMatched)
			{
				Close();
			}
			if (user.Pass != "")
			{
				return;
			}
			NewPasswordForm newPasswordForm = new NewPasswordForm();
			while (true)
			{
				if (newPasswordForm.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				if (user.PassLimited)
				{
					if (Account.IsPasswordValid(newPasswordForm.newPass))
					{
						user.Pass = newPasswordForm.newPass;
					}
					else
					{
						MessageBox.Show("Пароль не соответствует ограничениям.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						continue;
					}
				}
				else
				{
					user.Pass = newPasswordForm.newPass;
				}
				break;
			}
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			BinaryWriter binaryWriter = null;
			try
			{
				binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.Create, FileAccess.Write));
				foreach (Account u in users)
				{
					binaryWriter.Write(u.Login);
					binaryWriter.Write(u.Pass);
					binaryWriter.Write(u.Banned);
					binaryWriter.Write(u.PassLimited);
				}
				binaryWriter.Close();
			}
			catch
			{
				binaryWriter?.Close();
			}
		}
    }
}
