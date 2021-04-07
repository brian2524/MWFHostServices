using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HostServicesAPI.Objects
{
    public class GameInstanceAccessor
    {
        public GameInstanceAccessor(Process inGameInstanceProcess, GameInstanceModel inGameInstanceDatabaseModel)
        {
            gameInstanceProcess = inGameInstanceProcess;
            gameInstanceDatabaseModel = inGameInstanceDatabaseModel;
        }

        public Process gameInstanceProcess { get; set; }
        public GameInstanceModel gameInstanceDatabaseModel { get; set; }
    }
}
