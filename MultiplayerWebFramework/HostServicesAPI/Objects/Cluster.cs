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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))                                          // LINUX
            {
                Console.WriteLine("Hello Linux!");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))                                   // WINDOWS
            {
                switch (game)
                {
                    case Game.Game0:
                        {
                            Process newProcess = new Process()
                            {
                                StartInfo = new ProcessStartInfo()
                                {
                                    FileName = _configuration.GetValue<string>("GameFilePaths:ALSReplicated"),
                                    Arguments = args,
                                    CreateNoWindow = true,
                                    UseShellExecute = false
                                }
                            };
                            // Append specified port number at the end of arguments
                            newProcess.StartInfo.Arguments += (" -port=" + port);

                            if (newProcess.Start() == true)
                            {
                                /*return newGame;*/
                            }
                        }
                        break;
                    case Game.Game1:
                        break;
                }
            }

            return null;
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
