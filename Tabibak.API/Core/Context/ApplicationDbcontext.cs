using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tabibak.API.Core.Models;
using Tabibak.Core.Models;
using Tabibak.Models;

namespace Tabibak.Context
{

    public class ApplicationDbcontext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Patient> Patients { get; set; }

    }
}
