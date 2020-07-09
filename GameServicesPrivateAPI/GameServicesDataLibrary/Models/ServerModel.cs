using System;
using System.Collections.Generic;
using System.Text;

namespace GameServicesDataLibrary.Models
{
    public class ServerModel
    {
        public int Id { get; set; }
        public string ServerIP { get; set; }
        public string GameInstancesManagementApiIp { get; set; }
        public string GameInstancesManagementApiPort { get; set; }
        //public bool IsActive { get; set; }
    }
}
