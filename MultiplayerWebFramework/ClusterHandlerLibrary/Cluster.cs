using System;
using System.Collections.Generic;
using System.Text;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace ClusterHandlerLibrary
{
    public class Cluster
    {
        
        public List<GameInstanceModel> GameInstances { get; set; } // should this be a list? what type of operations will we be doing with this

        public GameInstanceModel SpinUp(Game game, string port, string args)
        {
            string localIp;
            try
            {
                localIp = GetMachineIP();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            GameInstanceModel newGameInstance = new GameInstanceModel
            {
                Id = -1,                // -1 for now until the database creates an entry for this game instance and returns its Id
                Game = game,
                Port = port,
                Args = args,
                HostId = 0              // Come up with a way for this host to know its id
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))                                          // LINUX
            {
                Console.WriteLine("Hello Linux!");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))                                   // WINDOWS
            {
                switch (newGameInstance.Game)
                {
                    case Game.Game0:
                        {
                            Process newProcess = new Process()
                            {
                                StartInfo = new ProcessStartInfo()
                                {
                                    // Appsettings.json GameFilePaths for Game0
                                    FileName = @"C:\Users\b2hin\Desktop\WindowsNoEditor\ALSReplicated\Binaries\Win64\ALSReplicatedServer.exe",
                                    Arguments = newGameInstance.Args,
                                    CreateNoWindow = true,
                                    UseShellExecute = false
                                }
                            };
                            // Append specified port number at the end of arguments
                            newProcess.StartInfo.Arguments += (" -port=" + newGameInstance.Port);

                            if (newProcess.Start() == true)
                            {

                            }
                        }
                        break;
                    case Game.Game1:
                        break;
                }
            }

            return newGameInstance;
        }

        private static string GetMachineIP()
        {
            string localIp;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIp = endPoint.Address.ToString();
            }

            return localIp;
        }
    }
}
