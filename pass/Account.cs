namespace pass
{
    public class Account
    {
		public string Login
		{
			get;
			set;
		}

		public string Pass
		{
			get;
			set;
		}

		public bool Banned
		{
			get;
			set;
		}

		public bool PassLimited
		{
			get;
			set;
		}

		public Account(string login, string pwd, bool ban, bool pwdLimit)
		{
			Login = login;
			Pass = pwd;
			Banned = ban;
			PassLimited = pwdLimit;
		}

		public static bool IsPasswordValid(string pwd)
		{
			for (int i = 0; i < pwd.Length; i++)
			{
				if (!char.IsLetter(pwd[i]) && pwd[i] != '-' && pwd[i] != '+' && pwd[i] != '*' && pwd[i] != '/' && pwd[i] != '%')
				{
					return false;
				}
			}
			return true;
		}
	}
}
