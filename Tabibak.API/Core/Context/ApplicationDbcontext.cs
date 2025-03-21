using Microsoft.AspNetCore.Identity;
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

            // ✅ Configure Many-to-Many Relationship
            modelBuilder.Entity<DoctorSpecialty>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecialtyId });

            modelBuilder.Entity<DoctorSpecialty>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecialties)
                .HasForeignKey(ds => ds.DoctorId);

            modelBuilder.Entity<DoctorSpecialty>()
                .HasOne(ds => ds.Specialty)
                .WithMany(s => s.DoctorSpecialties)
                .HasForeignKey(ds => ds.SpecialtyId);


            // ✅ Define relationships
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId)
                .HasPrincipalKey<ApplicationUser>(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .HasPrincipalKey<ApplicationUser>(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Explicitly define the length of UserId to match Identity column
            modelBuilder.Entity<Doctor>()
                .Property(d => d.UserId)
                .HasColumnType("nvarchar(450)");

            modelBuilder.Entity<Patient>()
                .Property(p => p.UserId)
                .HasColumnType("nvarchar(450)");
            SeedRoles(modelBuilder);
            SeedAdminUser(modelBuilder);



            modelBuilder.Entity<Appointment>()
            .Property(a => a.EndTime)
            .HasConversion(
                v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,  // Convert TimeOnly? -> TimeSpan?
                v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null  // Convert TimeSpan? -> TimeOnly?
            );
            modelBuilder.Entity<Appointment>()
           .Property(a => a.StartTime)
           .HasConversion(
               v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,  // Convert TimeOnly? -> TimeSpan?
               v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null  // Convert TimeSpan? -> TimeOnly?
           );
            base.OnModelCreating(modelBuilder);
        }
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Patient", NormalizedName = "PATIENT" },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }

        private void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var adminUser = new ApplicationUser
            {
                Id = "1001",
                UserName = "admin",
                FullName = "admin",
                Role = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true
            };

            // Hash the admin password
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign the Admin role to the user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1001", RoleId = "1" }
            );
        }
        public DbSet<FileStorage> FileStorages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<FavoriteDoctor> FavoriteDoctors { get; set; }
        public DbSet<Location> Locations { get; set; }

    }
}
