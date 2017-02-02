using System;
using System.IO;
using Server;
using MyCommon;
using System.Reflection;

namespace Server_Console
{
	  class MainClass
	  {
			static ServerCore server;
			static Logger logger = new Logger();
			static Config config = new Config();
			static string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.xml";

			public static void Main(string[] args)
			{
				  Setup(args);
				  if(server != null)
				  {
						Console.ReadLine();
						server.Run();
				  }
			}

			static void Setup(string[] args)
			{
				  Console.Title = "Hexalus Server";

				  bool dev = false;
				  bool debug = false;
				  bool useIO = false;

				  bool startServer = true;

				  //Load Arguments --<argument> [data]
				  for(int i = 0; i < args.Length; i++)
				  {
						switch(args[i])
						{
							  case "-h":
							  case "--help":
									Console.WriteLine(Console.Title);
									Console.WriteLine(RemoveThis.helpText);
									startServer = false;
									break;

							  case "--dev":
									dev = true;
									break;

							  case "--debug":
									debug = true;
									break;

							  case "--IO":
									useIO = true;
									break;

							  case "--config":
									if(args.Length > i)
									{
										  string path = args[i + 1];

										  if(File.Exists(path))
										  {
												configPath = path;
												i++;
										  }
										  else
										  {
												logger.Write("Incorrect Config Path : " + args[i + 1], Logger.logType.warm);
										  }
									}
									else
									{
										  logger.Write("Any Config Path", Logger.logType.warm);
									}
									break;

							  case "--force-config":
									if(args.Length > i)
									{
										  string path = args[i + 1];

										  configPath = path;
										  i++;
									}
									else
									{
										  logger.Write("Any Config Path", Logger.logType.warm);
									}
									break;

							  case "-v":
							  case "--version":
									Console.WriteLine(Console.Title + " v" + Assembly.GetEntryAssembly().GetName().Version);
									startServer = false;
									break;

							  default:
									logger.Write("Unknown Argument : " + args[i], Logger.logType.warm);
									break;
						}
				  }

				  if(startServer)
				  {

						config = XmlManager.Load<Config>(configPath, XmlManager.LoadMode.ReadCreateOrReplace, null, logger); //TODO Use config.xsd

						logger.Initialise(config.logPath, config.backColor, config.foreColor, config.logLevel, debug, dev,
										  new Logger.Outputs(true, !useIO, useIO, false, false));

						config.core.dev = dev;
						config.core.debug = debug;

						server = new ServerCore(
							config.core,
							logger
						);
				  }
			}
	  }
}
