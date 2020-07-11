using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerFrameworkModelsLibrary.Models
{
    /*A Host should have 2 Ip addresses, one reserved for the GameInstances to listen on for players (HostIp),
      and another for the HostServicesApi to listen on (AssociatedHostServicesApiIp). This way, players don't get access to the Api.*/
    class HostModel
    {
        public int Id { get; set; }
        public string HostIp { get; set; }
        public string AssociatedHostServicesApiIp { get; set; }
        public string AssociatedHostServicesApiPort { get; set; }
        public bool IsActive { get; set; }
    }
}
