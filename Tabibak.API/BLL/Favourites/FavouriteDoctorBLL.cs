using Microsoft.EntityFrameworkCore;
using Tabibak.Api.BLL;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.Api.BLL.Constants;
using Tabibak.API.Core.Models;
using Tabibak.Context;

namespace Tabibak.API.BLL.Favourites
{
    public class FavouriteDoctorBLL : BaseBLL, IFavouriteDoctorBLL
    {
        private readonly ApplicationDbcontext _context;

        public FavouriteDoctorBLL(ApplicationDbcontext context)
        {
            _context = context;
        }

        public async Task<IResponse<bool>> AddToFavoritesAsync(int patientId, int doctorId)
        {
            var response = new Response<bool>();

            // Check if the doctor is already in the patient's favorites
            bool alreadyExists = await _context.FavoriteDoctors
                .AnyAsync(f => f.PatientId == patientId && f.DoctorId == doctorId);

            if (alreadyExists)
                return response.CreateResponse(MessageCodes.AlreadyExists, nameof(FavoriteDoctor));

            // Add to favorites
            var favoriteDoctor = new FavoriteDoctor
            {
                PatientId = patientId,
                DoctorId = doctorId
            };

            _context.FavoriteDoctors.Add(favoriteDoctor);
            await _context.SaveChangesAsync();

            return response.CreateResponse(true);
        }

        public async Task<IResponse<bool>> RemoveFromFavoritesAsync(int patientId, int doctorId)
        {
            var response = new Response<bool>();

            var favoriteDoctor = await _context.FavoriteDoctors
                .FirstOrDefaultAsync(f => f.PatientId == patientId && f.DoctorId == doctorId);

            if (favoriteDoctor == null)
                return response.CreateResponse(MessageCodes.NotFound, nameof(FavoriteDoctor));

            _context.FavoriteDoctors.Remove(favoriteDoctor);
            await _context.SaveChangesAsync();

            return response.CreateResponse(true);
        }
    }
}
