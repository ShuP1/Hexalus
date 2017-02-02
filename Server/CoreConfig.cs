using System.Xml.Serialization;

namespace Server
{
	  public class CoreConfig
	  {
			public int port = 25001;

			public int size = 20;

			public int backlog = 0;

			public int kickTimer = 120;

			[XmlIgnore]
			public bool debug = false;

			[XmlIgnore]
			public bool dev = false;

			public CoreConfig() { }

			/*
				/// <summary>
				/// Initializes the server config
				/// </summary>
				/// <param name="Port">Listen port</param>
				/// <param name="Size">Max connected clients</param>
				/// <param name="Backlog">Max connections at the same time (0 = infite)</param>
				/// <param name="KickTimer">Time in seconds between connection test (0 = disable)</param>
				public CoreConfig(int Port = 25001, int Size = 20, int Backlog = 0, int KickTimer = 120, bool Debug = false, bool Dev = false)
				{
					  port = Port;
					  size = Size;
					  backlog = Backlog;
					  kickTimer = KickTimer;
					  debug = Debug;
					  dev = Dev;
				}
			*/
	  }
}

