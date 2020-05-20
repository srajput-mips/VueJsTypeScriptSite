
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackEnd.Services
{
    public interface ICats
    {
        Task<List<CatData>> GetCats();
    }
}