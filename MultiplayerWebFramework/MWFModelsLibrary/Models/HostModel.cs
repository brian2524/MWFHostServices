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


        //------------------------------------------------------------ This machine has 2 different IP addresses (dual NIC card). One exposed for players to join (with the game instance's corresponding port number) and one setup with firewall protection to only allow us to use
        // Players connect to this
        public string HostIp { get; set; }

        // We call on this to manage game instances. Players shouldn't be able to know this ip. We can spawn game instances and manage them with this.
        public string AssociatedHostServicesAPISocketAddress { get; set; }
        //public string AssociatedHostServicesApiIp { get; set; }
        //public string AssociatedHostServicesApiPort { get; set; }       // Get rid of this when I can! Just include the port number in the Ip address. We do however need to include a port number in the GameInstanceModel so we know which port the GameInstance is listening on.
        //------------------------------------------------------------
        public bool IsActive { get; set; }
    }
}
