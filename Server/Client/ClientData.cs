using System;

namespace Server.Client
{
	  public class ClientData
	  {
			public string username { get { return _username; } }
			protected string _username;

			public string salt { get { return _salt; } }
			protected string _salt;

			public string password { get { return _password; } }
			protected string _password;

			public ClientData(string Username, string Salt, string Password)
			{
				  _password = Password;
				  _salt = Salt;
				  _username = Username;
			}
	  }
}

