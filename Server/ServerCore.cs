using System;
using MyCommon;
using MyCommon.Generic;
using System.Reflection;
using System.Xml;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Common.Protocol;

namespace Server
{
	  public class ServerCore
	  {
			private bool _run = false;
			public bool run { get { return _run; } }

			private bool _open = false;
			public bool open { get { return _open; } }

			Logger logger;

			public CoreConfig config { get { return _config; } }
			CoreConfig _config;

			Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			byte[] byteData = new byte[2048];

			LinkTable<Socket, Client.Client> clients = new LinkTable<Socket, Client.Client>();
			object clientLock = new object();

			Timer kickerTimer;

			/// <summary>
			/// Setup the server
			/// </summary>
			public ServerCore(CoreConfig conf, Logger logs)
			{
				  logger = logs;
				  _config = conf;

				  logger.Write("Hexalus Server " + Assembly.GetEntryAssembly().GetName().Version, Logger.logType.fatal);

				  if(config.dev)
						logger.Write("SERVER IS IN DEV MODE !", Logger.logType.error, Logger.logDisplay.show);

				  if(config.debug)
						logger.Write("SERVER IS IN DEBUG MODE !", Logger.logType.error, Logger.logDisplay.show);

				  if(Type.GetType("Mono.Runtime") != null)
						logger.Write("Using Mono", Logger.logType.warm, Logger.logDisplay.show);
			}

			public void Run()
			{
				  //TODO Commands.Manager.Load();
				  logger.Write("Setting up server on *:" + config.port, Logger.logType.warm);
				  logger.Write("Size:" + config.size, Logger.logType.debug);
				  serverSocket.Bind(new IPEndPoint(IPAddress.Any, config.port));
				  serverSocket.Listen(config.backlog);
				  BeginAccept();
				  if(config.kickTimer != 0)
				  {
						TimeSpan tick = new TimeSpan(0, 0, config.kickTimer);
						kickerTimer = new Timer(KickerCallback, null, tick, tick);
				  }
				  logger.Write("Server setup complete", Logger.logType.info);
			}

			private void Stop()
			{
				  logger.Write("Stoping server", Logger.logType.warm, Logger.logDisplay.show);
				  kickerTimer.Dispose();
				  foreach(Socket socket in clients.Main.Keys)
				  {
						logger.Write("Shutdown " + GetName(socket), Logger.logType.info);
						socket.Shutdown(SocketShutdown.Both);
				  }
				  serverSocket.Close();
				  logger.Write("Server stoped", Logger.logType.info);
				  logger.Join();
			}

			private void OnAccept(IAsyncResult ar)
			{
				  try
				  {
						Socket clientSocket = serverSocket.EndAccept(ar);

						BeginAccept();

						clients.Add(clientSocket, new Client.Client());

						BeginReceive(clientSocket);
				  }
				  catch(Exception ex)
				  {
						logger.Write("Error during accept : " + ex.Message, Logger.logType.error);
				  }
			}

			private void OnReceive(IAsyncResult ar)
			{
				  try
				  {
						Socket clientSocket = (Socket)ar.AsyncState;
						int received;

						received = clientSocket.EndReceive(ar);

						var data = new byte[received];
						Array.Copy(byteData, data, received);

						Data packet = Data.FromBytes(ref data);

						if(packet != null)
						{
							  switch(packet.dtype)
							  {
									case DataTypes.Request:
										  RequestData request = (RequestData)packet;
										  Client.Client client;
										  RequestResult result = new RequestResult(ResultStatus.Error, ResultErrors.UnknownException.ToString());
										  if(clients.TryGetSecond(clientSocket, out client))
										  {
												try
												{
													  result = ExecuteRequest(client, request.type, request.data);
												}
												catch { }
										  }
										  else
										  {
												logger.Write("Can't find client for " + GetName(clientSocket), Logger.logType.error);
										  }
										  SendTo(clientSocket, new ResultData(request.id, result));
										  break;

									default:
										  logger.Write("Wrong packet type from " + GetName(clientSocket), Logger.logType.error);
										  break;
							  }
						}
						else
						{
							  logger.Write("Wrong packet form " + GetName(clientSocket), Logger.logType.error);
						}

						if(clients.ContainsMain(clientSocket))
						{
							  BeginReceive(clientSocket);
						}
						else
						{
							  clientSocket.Shutdown(SocketShutdown.Both);
						}

				  }
				  catch(Exception ex)
				  {
						logger.Write("Error during receive : " + ex.Message, Logger.logType.error);
				  }
			}

			private void KickerCallback(object o)
			{
				  lock(clientLock)
				  {
						foreach(Socket current in clients.Main.Keys)
						{
							  try
							  {
									if((current.Poll(10, SelectMode.SelectRead) && current.Available == 0) || !current.Connected)
									{
										  lock(clientLock)
										  {
												if(clients.ContainsMain(current))
													  clients.RemoveMain(current);
										  }
										  try
										  {
												current.Shutdown(SocketShutdown.Both);
										  }
										  catch { }
									}
							  }
							  catch { }
						}
				  }
			}

			private RequestResult ExecuteRequest(Client.Client client, RequestTypes type, string data)
			{
				  switch(type)
				  {
						case RequestTypes.Leave:
							  lock(clientLock)
							  {
									if(!clients.ContainsSecond(client))
										  return new RequestResult(ResultStatus.Error, ResultErrors.Disconnected.ToString());

									clients.RemoveSecond(client);
									return new RequestResult(ResultStatus.OK);
							  }

						case RequestTypes.IdentificationStart:
							  return new RequestResult(ResultStatus.Error, ResultErrors.WIP.ToString());

						case RequestTypes.IdentificationEnd:
							  return new RequestResult(ResultStatus.Error, ResultErrors.WIP.ToString());

						default:
							  return new RequestResult(ResultStatus.Error, ResultErrors.UnknownType.ToString()); //TODO log???

				  }
			}

			private void SendTo(Socket soc, Data data)
			{
				  try
				  {
						if(config.debug)
							  logger.Write("Send Data To " + GetName(soc), Logger.logType.debug);

						soc.Send(data.ToBytes(), SocketFlags.None);
				  }
				  catch { }
			}

			private void BroadCast(Data data)
			{
				  foreach(Socket soc in clients.Main.Keys)
				  {
						SendTo(soc, data);
				  }
			}

			private string GetName(Socket soc)
			{
				  Client.Client client;
				  if(clients.TryGetSecond(soc, out client))
						if(client.data != null)
							  return client.data.username;

				  try
				  {
						return ((IPEndPoint)soc.LocalEndPoint).Address.ToString();
				  }
				  catch
				  {
						return "?";
				  }
			}

			private void BeginAccept()
			{
				  serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
			}

			private void BeginReceive(Socket soc)
			{
				  soc.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), soc);
			}
	  }
}