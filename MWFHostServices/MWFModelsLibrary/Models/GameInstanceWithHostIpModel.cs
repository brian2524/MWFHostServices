using MWFModelsLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MWFModelsLibrary.Models
{
    public class GameInstanceWithHostIpModel
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Args { get; set; }
        public string HostIp { get; set; }
    }
}
