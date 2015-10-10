using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dentist.Models.Doctor;
using Dentist.Models.Patient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

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
        public WriteContext()
            : base()
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }

        public bool TrySaveChanges(out string errorMessage)
        {
            errorMessage = "";
            var modelState = new ModelStateDictionary();
            var changesSaved = TrySaveChanges(modelState);
            if (!changesSaved)
            {
                errorMessage = string.Join("; ", modelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
            }
            return changesSaved;
        }

        public bool TrySaveChanges(ModelStateDictionary modelState)
        {
            bool hasError = GetValidationErrors().Any();

            if (hasError)
            {
                AddModelErrors(this, modelState);
                return false;
            }

            base.SaveChanges();
            return true;
        }

        public override int SaveChanges()
        {
            throw new Exception("Please make use of TrySaveChanges method with modelState and writeContext parameter to pass errors from business layer to modestate");
            return base.SaveChanges();
        }

        private void AddModelErrors(ApplicationDbContext writeContext, ModelStateDictionary modelState)
        {
            var dbEntityValidationResults = writeContext.GetValidationErrors();
            foreach (var dbEntityValidationResult in dbEntityValidationResults)
            {
                var validationErrors = dbEntityValidationResult.ValidationErrors;
                foreach (var dbValidationError in validationErrors)
                {
                    modelState.AddModelError(string.Empty, dbValidationError.ErrorMessage);
                }
            }
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
            var applicationDbContext = new ApplicationDbContext();
            applicationDbContext.Database.CreateIfNotExists();
            return applicationDbContext;
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

        public System.Data.Entity.DbSet<Patient.Patient> Patients { get; set; }
        public System.Data.Entity.DbSet<Doctor.Doctor> Doctors { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.Appointment> Appointments { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailability> DailyAvailabilities { get; set; }

        public System.Data.Entity.DbSet<Dentist.Models.DailyAvailabilitySetting> DailyAvailabilitySettings { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.CalenderSetting> CalenderSettings { get; set; }
        public System.Data.Entity.DbSet<Dentist.Models.File> Files { get; set; }
        public System.Data.Entity.DbSet<CareService> CareServices { get; set; }
        public System.Data.Entity.DbSet<Membership> Memberships { get; set; }
        public System.Data.Entity.DbSet<Specialization> Specializations { get; set; }
        public System.Data.Entity.DbSet<Qualification> Qualifications { get; set; }
        public System.Data.Entity.DbSet<Experience> Experiences { get; set; }
        public System.Data.Entity.DbSet<Award> Awards { get; set; }
        public System.Data.Entity.DbSet<VitalSign> VitalSigns { get; set; }
        public System.Data.Entity.DbSet<PatientNote> PatientNotes { get; set; }
        public System.Data.Entity.DbSet<Note> Notes { get; set; }
        public System.Data.Entity.DbSet<NoteType> NoteTypes { get; set; }
        public System.Data.Entity.DbSet<TreatmentPlan> TreatmentPlans { get; set; }
        public System.Data.Entity.DbSet<Treatment> Treatments { get; set; }
   }


}