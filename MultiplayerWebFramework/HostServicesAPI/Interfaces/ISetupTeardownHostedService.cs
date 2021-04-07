using Microsoft.Extensions.Hosting;
using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostServicesAPI.Interfaces
{
    interface ISetupTeardownHostedService : IHostedService
    {
        public HostModel applicationHostModel { get; }
    }
}
