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
				
				// I couldn't figure out how to GET an object that wasn't wrapped in IEnum
				// If anyone knows a better solution let me know
				//
				// However, all you have to do to create an endpoint is: 
				// 1. Take this template rename ApiExampleController to your Controller name 
				// 2. Create a function like that below with your DTO object that will be created
				//    in Controllers/Models/ where you'll find a sample DTO object CustomObjectDTO
				// 3. Change "Get()" to the type of HTTP request you wish to create.
				//
				// Your endpoint will be https://localhost:<port>/<lower-case-controller-name>/<type-of-request>
				//
				// eg: https://localhost:XXXX/apiexample/get
				// To check in command line curl https://localhost:5001/apiexample/get -k
        [HttpGet("apiexample/get")]
        public IEnumerable<CustomObjectDTO> Get()
        {
					return Enumerable.Range(1,1).Select(index => 
						new CustomObjectDTO   
							{ 
								message = "Congrats, you called an endpoint" 
							}
					);
        }
    }
}
