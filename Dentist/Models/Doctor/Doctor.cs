using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using System.Web.Services.Description;
using AutoMapper;
using Dentist.Enums;
using Dentist.Models.Tags;

namespace Dentist.Models.Doctor
{
    public class Doctor : Person, IModelWithWriteContext
    {
        public Doctor() :base()
        {
            PersonRole = PersonRole.Doctor;
            // following lists has to be created for new objects to work
            Practices = new List<Practice>(); // lookup (M:M)
            Services = new List<CareService>(); //lookup
            Specializations = new List<Specialization>(); //lookup
            Memberships = new List<Membership>(); //lookup

            DailyAvailabilities = new List<DailyAvailability>(); //owner
            Appointments = new List<Appointment>(); //owner
            Qualifications = new List<Qualification>(); //owner (M:1)
            Experiences = new List<Experience>(); //owner
            Awards = new List<Award>(); //owner
            //Registration = new Registration(); //owner (nullable 1:M) they should not be created in ctor for ef to work
        }

        [StringLength(100)]
        public string About { get; set; }
        public virtual List<CareService> Services { get; private set; }
        public virtual List<Specialization> Specializations { get; private set; }
        public virtual List<Qualification> Qualifications { get; private set; }
        public virtual List<Experience> Experiences { get; private set; }
        public virtual List<Award> Awards { get; private set; }
        public virtual List<Membership> Memberships { get; private set; }
        public int? ExperienceInYears { get; set; }
        
        public virtual Registration Registration { get; set; }
        public int? RegistrationId { get; set; }

        [NotMapped]
        public WriteContext Context { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; private set; }
        
        [Required]
        public virtual ICollection<Practice> Practices { get; private set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; private set; }

        [StringLength(10)]
        public string Color { get; set; }

        private Practice LoadPractice(int practiceId)
        {
            return Context.Practices.Find(practiceId);
        }

        public void AddPractices(List<int> practiceIdsToAdd)
        {
            practiceIdsToAdd.ForEach(AddPractice);
        }

        private void AddPractice(int practiceId)
        {
            Practice practice = LoadPractice(practiceId);
            AddPractice(practice);
        }

        private void AddPractice(Practice practice)
        {
            // note the add and delete only works against context but not against list
            Practices.Add(practice);
            List<DailyAvailabilitySetting> dailyAvailabilitySettings = GetDailyAvailabilitySetting();
            foreach (DailyAvailabilitySetting dailyAvailabilitySetting in dailyAvailabilitySettings)
            {
                var dailyAvailability = new DailyAvailability
                {
                    Doctor = this,
                    Practice = practice
                };
                Mapper.Map(dailyAvailabilitySetting, dailyAvailability);
                DailyAvailabilities.Add(dailyAvailability);
            }
        }

        public void RemovePractices(List<int> practiceIdsToRemove)
        {
            practiceIdsToRemove.ForEach(RemovePractice);
        }

        private void RemovePractice(int practiceId)
        {
            Practice practice = Practices.First(x => x.Id == practiceId);
            RemovePractice(practice);
        }

        private void RemovePractice(Practice practice)
        {
            IEnumerable<DailyAvailability> daily = DailyAvailabilities.Where(x => x.PracticeId == practice.Id);
            // Note remove range on the list does not work
            Context.DailyAvailabilities.RemoveRange(daily);
            Practices.Remove(practice);
        }
        
        private CareService LoadService(int serviceId)
        {
            return Context.CareServices.Find(serviceId);
        }

        public void AddServices(List<int> serviceIdsToAdd)
        {
            serviceIdsToAdd.ForEach(AddService);
        }

        private void AddService(int serviceId)
        {
            CareService careService = LoadService(serviceId);
            Services.Add(careService);
        }

        public void RemoveServices(List<int> serviceIdsToRemove)
        {
            serviceIdsToRemove.ForEach(RemoveService);
        }

        private void RemoveService(int serviceId)
        {
            CareService careService = Services.Find(x => x.Id == serviceId);
            Services.Remove(careService);
        }

        private Membership LoadMembership(int membershipId)
        {
            return Context.Memberships.Find(membershipId);
        }

        public void AddMemberships(List<int> membershipIdsToAdd)
        {
            membershipIdsToAdd.ForEach(AddMembership);
        }

        private void AddMembership(int membershipId)
        {
            Membership membership = LoadMembership(membershipId);
            Memberships.Add(membership);
        }

        public void RemoveMemberships(List<int> membershipIdsToRemove)
        {
            membershipIdsToRemove.ForEach(RemoveMembership);
        }

        private void RemoveMembership(int membershipId)
        {
            Membership membership = Memberships.Find(x => x.Id == membershipId);
            Memberships.Remove(membership);
        }

        private Specialization LoadSpecialization(int specializationId)
        {
            return Context.Specializations.Find(specializationId);
        }

        public void AddSpecializations(List<int> specializationIdsToAdd)
        {
            specializationIdsToAdd.ForEach(AddSpecialization);
        }

        private void AddSpecialization(int specializationId)
        {
            var specialization = LoadSpecialization(specializationId);
            Specializations.Add(specialization);
        }

        public void RemoveSpecializations(List<int> specializationIdsToRemove)
        {
            specializationIdsToRemove.ForEach(RemoveSpecialization);
        }

        private void RemoveSpecialization(int specializationId)
        {
            var specialization = Specializations.Find(x => x.Id == specializationId);
            Specializations.Remove(specialization);
        }

        public void AddQualification(Qualification qualificationToAdd)
        {
            qualificationToAdd.Doctor = this;
            this.Qualifications.Add(qualificationToAdd);
        }

        public void AddExperience(Experience experienceToAdd)
        {
            experienceToAdd.Doctor = this;
            this.Experiences.Add(experienceToAdd);
        }

        public void AddAward(Award awardToAdd)
        {
            awardToAdd.Doctor = this;
            this.Awards.Add(awardToAdd);
        }

        private List<DailyAvailabilitySetting> GetDailyAvailabilitySetting()
        {
            return Context.DailyAvailabilitySettings.ToList();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Context == null)
            {
                throw new Exception("Context missing in doctor");
            }

            List<ValidationResult> results = base.Validate(validationContext).ToList();            

            // Lazy loading is turned off by EF during validation therefore load the practicies manually
            var isNewObj = Id == 0;// cannot load practices for the doctor which is new
            if ((!Context.Entry(this).Collection(p => p.Practices).IsLoaded) && (!isNewObj))
            {
                Context.Entry(this).Collection(p => p.Practices).Load();
            }
            if (this.Practices.Count == 0)
            {
                results.Add(new ValidationResult("Doctor can not be registered without a practice"));
            }

            // Note qualification entity will validate itself
            return results;
        }
    }

}