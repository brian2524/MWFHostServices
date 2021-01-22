using System;
using System.Collections.Generic;
using System.Text;

namespace MWFModelsLibrary.Models
{
    class GameModel
    {
        public string Name { get; set; }
        //  This should be enum instead with dropdown for choosing
        public string Game { get; set; }
        public string Map { get; set; }
        public string Args { get; set; }
    }
}
