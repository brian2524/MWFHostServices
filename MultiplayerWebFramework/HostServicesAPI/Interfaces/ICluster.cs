using Microsoft.AspNetCore.Mvc;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HostServicesAPI.Interfaces
{
    public interface ICluster
    {
        public Task<HttpResponseMessage> SpinUp(Game game, string port, string args);
    }
}
