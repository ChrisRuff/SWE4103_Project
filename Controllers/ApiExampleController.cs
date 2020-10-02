using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Controllers.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Controllers
{
		// api/ApiExample/get
    [ApiController]
    public class ApiExampleController : ControllerBase
    {
        private readonly ILogger<ApiExampleController> _logger;

        public ApiExampleController(ILogger<ApiExampleController> logger)
        {
            _logger = logger;
        }
				
				// However, all you have to do to create an endpoint is: 
				// 1. Take this template rename ApiExampleController to your Controller name 
				// 2. Create a function like that below with your DTO object that will be created
				//    in Controllers/Models/ where you'll find a sample DTO object CustomObjectDTO
				// 3. Change "Get()" to the type of HTTP request you wish to create.
				//
				// Your endpoint will be https://localhost:<port>/api/<lower-case-controller-name>/<type-of-request>
				//
				// eg: https://localhost:XXXX/api/apiexample/get
				// To check in command line curl https://localhost:5001/api/apiexample/get -k
        [HttpGet, Route("api/apiexample/get")]
        public List<CustomObjectDTO> Get()
        {
					List<CustomObjectDTO> customObjects = new List<CustomObjectDTO>();
					
					customObjects.Add(new CustomObjectDTO   
          { 
          	message = "Congrats, you called an endpoint" 
          });

					return customObjects;
        }
    }
}
