using MWFModelsLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MWFModelsLibrary.Models
{
    public class GameInstanceModel
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Args { get; set; }
        // Maybe implement is so this will be the associated host Id so we can do join statements
        public string AssociatedHost { get; set; }
    }
}
