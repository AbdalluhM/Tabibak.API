using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Core.Models;
using Tabibak.Context;

namespace Tabibak.API.BLL.Specialties
{
    public class SpecialtyBLL : ISpecialtyBLL
    {
        private readonly ApplicationDbcontext _dbcontext;

        public SpecialtyBLL(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }



        public async Task<IResponse<List<Specialty>>> GetSpecialties()
        {
            var response = new Response<List<Specialty>>();
            try
            {
                return response.CreateResponse(await _dbcontext.Specialties.ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
