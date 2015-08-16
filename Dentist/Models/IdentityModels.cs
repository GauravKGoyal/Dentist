using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dentist.Models.Doctor;
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


    public class WriteContext : ApplicationDbContext
    {
        public WriteContext():base()
        {

        }

        public bool TrySaveChanges(ModelStateDictionary modelState)
        {
            bool hasError = AddModelErrors(modelState, this);
            if (hasError)
            {
                return false;
            }            

            base.SaveChanges();
            return true;
        }

        public bool TrySaveChanges()
        {
            base.SaveChanges();
            return true;
        }

        public override int SaveChanges()
        {
            throw new Exception("Please make use of TrySaveChanges method with modelState and writeContext parameter to pass errors from business layer to modestate");
            return base.SaveChanges();
        }

        private bool AddModelErrors(ModelStateDictionary modelState, ApplicationDbContext writeContext)
        {
            var hasError = false;
            var dbEntityValidationResults = writeContext.GetValidationErrors();
            foreach (var dbEntityValidationResult in dbEntityValidationResults)
            {
                var validationErrors = dbEntityValidationResult.ValidationErrors;
                foreach (var dbValidationError in validationErrors)
                {
                    modelState.AddModelError(string.Empty, dbValidationError.ErrorMessage);
                    hasError = true;
                }
            }
            return hasError;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
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
        
        [Obsolete]        
        public override int SaveChanges()
        {
            bool isReadOnlyContext = (this.Configuration.ProxyCreationEnabled == false);
            if (isReadOnlyContext)
            {
                throw new Exception("Please use write context");
            }            
            return base.SaveChanges();
        }

        public System.Data.Entity.DbSet<Dentist.Models.Practice> Practices { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Paitient> Paitients { get; set; }
        public System.Data.Entity.DbSet<Doctor.Doctor> Doctors { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Appointment> Appointments { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailability> DailyAvailabilities { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailabilitySetting> DailyAvailabilitySettings { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.CalenderSetting> CalenderSettings { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.File> Files { get; set; }
        public System.Data.Entity.DbSet<Service> Services { get; set; }
    }

   
}