using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Controllers.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend
{
    [ApiController]
    public class ProfController : ControllerBase
    {
        private readonly ILogger<ProfController> _logger;

        public ProfController(ILogger<ProfController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("api/prof/add")]
        public List<ProfDTO> AddProf(List<ProfDTO> profs)
        {
            for (int i = 0; i < profs.Count; i++)
            {
                bool res = DatabaseConnector.Connector.AddProf(profs[i].profName, profs[i].email, profs[i].pass);
                profs[i].response = res;
            }
            return profs;
        }

        [HttpPost, Route("api/prof/login")]
        public List<ProfDTO> LoginProf(List<ProfDTO> profs)
        {
            for (int i = 0; i < profs.Count; i++)
            {
                bool res = DatabaseConnector.Connector.CheckPassProf(profs[i].email, profs[i].pass);
                profs[i].response = res;
            }
            return profs;
        }
    }
}