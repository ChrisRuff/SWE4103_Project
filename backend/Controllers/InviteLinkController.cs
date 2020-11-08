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
    public class InviteLinkController : ControllerBase
    {
        private readonly ILogger<InviteLinkController> _logger;

        public InviteLinkController(ILogger<InviteLinkController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("api/invite_link/add")]
        public List<ClassDTO> GenerateClassCodeAPI(List<ClassDTO> classDTOs)
        {
            for (int i = 0; i < classDTOs.Count; i++)
            {
                try
                {
                    string res = DatabaseConnector.Connector.GenerateInviteKey(classDTOs[i].className);
                    classDTOs[i].classCode = res;
                    classDTOs[i].response = true;
                }
                catch (Exception)
                {
                    classDTOs[i].response = false;
                }
            }
            return classDTOs;
        }

        [HttpPost, Route("api/invite_link/get")]
        public List<ClassDTO> GetClassCodeAPI(List<ClassDTO> classDTOs)
        {
            for (int i = 0; i < classDTOs.Count; i++)
            {
                try
                {
                    string res = DatabaseConnector.Connector.GetInviteKey(classDTOs[i].classCode);
                    classDTOs[i].classCode = res;
                    classDTOs[i].response = true;
                }
                catch (Exception)
                {
                    classDTOs[i].response = false;
                }
            }
            return classDTOs;
        }
    }
}