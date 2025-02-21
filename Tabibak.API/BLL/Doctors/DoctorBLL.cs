using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Dtos.Specialities;
using Tabibak.Context;

namespace Tabibak.API.BLL.Doctors
{
    public class DoctorBLL : IDoctorBLL
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IMapper _mapper;

        public DoctorBLL(IMapper mapper, ApplicationDbcontext dbcontext)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
        }

        public Task<IResponse<DoctorDetailResultDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<List<GetDoctoctorBySpecialtyDto>>> GetDoctorsBySpecialtyAsync(int specialtyId)
        {
            var response = new Response<List<GetDoctoctorBySpecialtyDto>>();
            try
            {
                var doctors = await _dbcontext.Doctors
                    .Where(d => d.DoctorSpecialties.Any(s => s.SpecialtyId == specialtyId))  // ✅ FIXED QUERY
                    .Include(d => d.User)  // ✅ Ensure ApplicationUser data is loaded
                    .ToListAsync();

                return response.CreateResponse(_mapper.Map<List<GetDoctoctorBySpecialtyDto>>(doctors));
            }
            catch (Exception e)
            {
                throw;  // Keep for debugging
            }
        }

    }
}
