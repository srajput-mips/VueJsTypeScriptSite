using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System.Net.Http;
using BackEnd.Config;
using BackEnd.Models;
using System.Text.RegularExpressions;

//for unit testing secure content
[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("BackEnd.Tests")]

namespace BackEnd.Services.Implementations
{
    public class BackEndSvc : IBackEnd
    { 
        ICats _cats; 

        //constructor injection
        public BackEndSvc(ICats cats)
        { 
            _cats = cats;            

        }

        public async  Task<List<CatData>> GetCats()
        {
            return await _cats.GetCats();
        }

 
    }
}
