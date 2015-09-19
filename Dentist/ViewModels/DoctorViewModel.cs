using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Dentist.Helpers;
using Dentist.Models;
using System.Linq;
using Dentist.Models.Doctor;

namespace Dentist.ViewModels
{
    public class DoctorViewModel : PersonViewModel
    {
        public DoctorViewModel()
        {
            Practices = new List<int>();
            Services = new List<int>();
            Memberships = new List<int>();
            Specializations = new List<int>();
        }

        public bool IsDoctor
        {
            get { return true; }
        }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "Experience in years")]
        public int? ExperienceInYears { get; set; }

        public RegistrationViewModel Registration { get; set; }

        [Display(Name = "Appointment Color")]
        public string Color { get; set; }

        [RequiredListItem]
        public List<int> Practices { get; set; }

        private List<int> PracticeIdsToRemove(Doctor doctor)
        {
            return doctor.Practices.Where(practice => !Practices.Contains(practice.Id)).Select(x => x.Id).ToList();
        }

        private List<int> PracticeIdsToAdd(Doctor doctor)
        {
            return Practices.Where(practiceId => doctor.Practices.All(practice => practice.Id != practiceId))
                    .ToList();    
        }

        public List<int> Services { get; set; }

        private List<int> ServiceIdsToRemove(Doctor doctor)
        {
            return doctor.Services.Where(service => !Services.Contains(service.Id)).Select(x => x.Id).ToList();
        }

        private List<int> ServiceIdsToAdd(Doctor doctor)
        {
            return Services.Where(serviceId => doctor.Services.All(service => service.Id != serviceId))
                    .ToList();
        }

        public List<int> Memberships { get; set; }

        private List<int> MembershipsToRemove(Doctor doctor)
        {
            return doctor.Memberships.Where(membership => !Memberships.Contains(membership.Id)).Select(x => x.Id).ToList();
        }

        private List<int> MembershipIdsToAdd(Doctor doctor)
        {
            return Memberships.Where(id => doctor.Memberships.All(membership => membership.Id != id))
                    .ToList();
        }

        public List<int> Specializations { get; set; }

        private List<int> SpecializationsToRemove(Doctor doctor)
        {
            return doctor.Specializations.Where(specialization => !Specializations.Contains(specialization.Id)).Select(x => x.Id).ToList();
        }

        private List<int> SpecializationIdsToAdd(Doctor doctor)
        {
            return Specializations.Where(id => doctor.Specializations.All(specialization => specialization.Id != id))
                    .ToList();
        }

        public void CopyTo(Doctor doctor)
        {
             base.CopyTo(doctor);
             Mapper.DynamicMap(this, doctor);

             doctor.RemovePractices(PracticeIdsToRemove(doctor));
             doctor.AddPractices(PracticeIdsToAdd(doctor));
             doctor.RemoveServices(ServiceIdsToRemove(doctor));
             doctor.AddServices(ServiceIdsToAdd(doctor));
             doctor.RemoveMemberships(MembershipsToRemove(doctor));
             doctor.AddMemberships(MembershipIdsToAdd(doctor));
             doctor.RemoveSpecializations(SpecializationsToRemove(doctor));
             doctor.AddSpecializations(SpecializationIdsToAdd(doctor));

             if (UploadedAvatarFile != null)
             { 
                doctor.Files.Add(UploadedAvatarFile);
             }
        }

        public void CopyFrom(Doctor doctor)
        {
            base.CopyFrom(doctor);
            Mapper.DynamicMap(doctor, this);
        }
    }
}