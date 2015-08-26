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
            Practices = new List<Practice>(); // lookup
            Services = new List<CareService>(); //lookup
            Specializations = new List<Specialization>(); //lookup
            Memberships = new List<Membership>(); //lookup

            DailyAvailabilities = new List<DailyAvailability>(); //owner
            Appointments = new List<Appointment>(); //owner
            Qualification = new List<Qualification>(); //owner
            Experiences = new List<Experience>(); //owner
            Awards = new List<Award>(); //owner
            //   Registration = new Registration(); //owner
        }

        public string About { get; set; }
        public virtual List<CareService> Services { get; set; }
        public virtual List<Specialization> Specializations { get; set; }
        public virtual List<Qualification> Qualification { get; set; }
        public virtual List<Experience> Experiences { get; set; }
        public virtual List<Award> Awards { get; set; }
        public virtual List<Membership> Memberships { get; set; }

        public int? RegistrationId { get; set; }
        public Registration Registration { get; set; }

        [NotMapped]
        public WriteContext Context { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; private set; }

        public virtual List<Practice> Practices { get; private set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; private set; }

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
            Practice practice = Practices.Find(x => x.Id == practiceId);
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
            return Context.Services.Find(serviceId);
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

        private List<DailyAvailabilitySetting> GetDailyAvailabilitySetting()
        {
            return Context.DailyAvailabilitySettings.ToList();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = base.Validate(validationContext).ToList();

            if (Practices.Count == 0)
            {
                results.Add(new ValidationResult("Doctor can not be registered without a practice"));
            }

            return results;
        }
    }
}