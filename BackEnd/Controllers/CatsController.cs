using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Swashbuckle.AspNetCore.Filters;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatsController : ControllerBase
    {
        //inject the service
        private readonly IBackEnd _svc; 

      
        public CatsController(IBackEnd svc)
        { 
            _svc = svc; 
        }

        /// <summary>
        ///Function to get cats from the api
        /// </summary>
        [HttpGet] 
        public async Task<List<CatData>> Get()
        {
             return await _svc.GetCats(); 
        }
      
    } 
} 