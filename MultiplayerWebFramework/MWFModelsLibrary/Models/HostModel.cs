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
        // Players connect to this ip. Host model doesn't provide the port the game is listening on. GameInstanceModel does
        public string HostIp { get; set; }

        // We call on this to manage game instances. Players shouldn't be able to know this ip. We can spawn game instances and manage them with this.
        public string HostServicesAPISocketAddress { get; set; }
        //------------------------------------------------------------
        public bool IsActive { get; set; }
    }
}
