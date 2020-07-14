using MultiplayerFrameworkModelsLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MultiplayerFrameworkModelsLibrary.Models
{
    public class GameInstanceModel
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Args { get; set; }
        public string AssociatedServer { get; set; }
    }
}
