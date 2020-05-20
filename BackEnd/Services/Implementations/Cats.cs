using BackEnd.Config;
using BackEnd.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//for unit testing
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace BackEnd.Services.Implementations
{
    internal class Cats : ICats
    {

        private readonly HttpClient _httpClient;
        private readonly ConfigSettings _urls;
        readonly AsyncRetryPolicy<HttpResponseMessage> _httpRetryPolicy;
        readonly AsyncTimeoutPolicy _timeoutPolicy;

        //constructor injection
        public Cats(HttpClient httpClient, IOptions<ConfigSettings> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;

            //handle re-tries for http failures (can be pushed to start up at time of registeration)
            _httpRetryPolicy =
                 Policy.HandleResult<HttpResponseMessage>(msg => msg.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                 .Or<TimeoutRejectedException>()
                 .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //set timeout policy
            _timeoutPolicy = Policy.TimeoutAsync(25);

        }

        public async Task<List<CatData>> GetCats()
        {
            // Get cats
            try
            {
                var Response = await _httpClient.GetStringAsync(_urls.CatsBaseURL);
              
                var Owners = JsonConvert.DeserializeObject<List<PetOwner>>(Response);

                return (from o in Owners.Where(x => x.Pets != null).GroupBy(g => g.Gender)
                        select new CatData
                        {
                            Gender = o.First().Gender,
                            Names = o.SelectMany(i => i.Pets.Where(j =>
                              !string.IsNullOrWhiteSpace(j.Type) &&
                               j.Type.ToUpper().Equals("CAT"))).OrderBy(x => x.Name).Select(y => y.Name).ToArray()
                        }).ToList();

            }catch(Exception)
            {
                return null; /// or throw 404 etc
            }
        }

    }
}
