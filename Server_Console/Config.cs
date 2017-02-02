using System;
using System.Xml.Serialization;
using Server;
using MyCommon;

namespace Server_Console
{
	  //[XmlRoot("Config")]
	  public class Config
	  {
			public CoreConfig core = new CoreConfig();
			public string logPath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
			public Logger.logType logLevel = Logger.logType.info;
			public ConsoleColor[] backColor = new ConsoleColor[6] { ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Red };
			public ConsoleColor[] foreColor = new ConsoleColor[6] { ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.White };

			public Config() { }
	  }
}