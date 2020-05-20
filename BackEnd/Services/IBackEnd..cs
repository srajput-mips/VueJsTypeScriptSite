
using BackEnd.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public interface IBackEnd
    {

        Task<List<CatData>> GetCats();

 
    }
}
