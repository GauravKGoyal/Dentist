using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dentist.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<Dentist.Models.Practice> Practices { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Paitient> Paitients { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.Doctor> Doctors { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Appointment> Appointments { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailability> DailyAvailabilities { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailabilitySetting> DailyAvailabilitySettings { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.CalenderSetting> CalenderSettings { get; set; }
    }
}