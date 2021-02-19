using System;
using System.Collections.Generic;
using System.Text;

namespace MWFModelsLibrary.Models
{
    /*A Host should have 2 Ip addresses, one reserved for the GameInstances to listen on for players (HostIp),
      and another for the HostServicesApi to listen on (AssociatedHostServicesApiIp). This way, players don't get access to the Api.*/
    public class HostModel
    {
        public int Id { get; set; }

        // Players connect to this
        public string HostIp { get; set; }

        // This ip is only for us. Players shouldn't be able to know this ip. We can spawn game instances and manage them with this.
        public string AssociatedHostServicesApiIp { get; set; }
        public string AssociatedHostServicesApiPort { get; set; }
        public bool IsActive { get; set; }
    }
}
