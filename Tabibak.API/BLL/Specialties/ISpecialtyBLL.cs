using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.Core.Models;

namespace Tabibak.API.BLL.Specialties
{
    public interface ISpecialtyBLL
    {
        Task<IResponse<List<Specialty>>> GetSpecialties();

    }
}
