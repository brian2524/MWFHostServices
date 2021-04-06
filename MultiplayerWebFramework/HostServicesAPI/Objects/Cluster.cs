using HostServicesAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HostServicesAPI.Objects
{
    public class Cluster : ICluster
    {
        public List<GameInstanceModel> GameInstances { get; set; }

        private readonly IConfiguration _configuration;
        public Cluster(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }
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
                                    FileName = _configuration.GetValue<string>("GameFilePaths:ALSReplicated"),
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
