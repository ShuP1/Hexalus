using System;

namespace Server.Client
{
	  public class Client
	  {
			public enum Status { Linked, Connected }

			public ClientData data { get { return _data; } }
			protected ClientData _data;

			public string sessionSalt { get { return _sessionSalt; } }
			protected string _sessionSalt;

			public Status status { get { return _status; } }
			protected Status _status;

			public Client()
			{
				  _sessionSalt = StringManager.RandomString(16);
				  _status = Status.Linked;
			}

			public void SetData(ClientData Data)
			{
				  _data = Data;
			}
	  }
}

