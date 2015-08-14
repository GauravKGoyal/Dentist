using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using System.Linq;

namespace Dentist.Models
{
    public class Doctor : Person
    {
        public Doctor() : base()
        {
            PersonRole = Enums.PersonRole.Doctor;
            Practices = new List<Practice>(); // lookup
            Services = new List<Service>(); //lookup
            Specializations = new List<Specialization>(); //lookup
            Memberships = new List<Membership>(); //lookup

            DailyAvailabilities = new List<DailyAvailability>(); //owner
            Appointments = new List<Appointment>(); //owner
            Qualification = new List<Qualification>();//owner
            Experiences = new List<Experience>(); //owner
            Awards = new List<Award>(); //owner
         //   Registration = new Registration(); //owner

        }

        public string About { get; set; }

        public virtual List<Service> Services { get; set; }
        public virtual List<Specialization> Specializations { get; set; }
        public virtual List<Qualification> Qualification { get; set; }
        public virtual List<Experience> Experiences { get; set; }
        public virtual List<Award> Awards { get; set; }
        public virtual List<Membership> Memberships { get; set; }

        public int? RegistrationId { get; set; }
        public Registration Registration { get; set; }


        [NotMapped]
        public ApplicationDbContext Context { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; private set; }

        public virtual List<Practice> Practices { get; private set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; private set; }

        public string Color { get; set; }

        private Practice LoadPractice(int practiceId)
        {
            return Context.Practices.Find(practiceId);
        }

        public bool PracticeExists(int practiceId)
        {
            return Practices.Exists(x => x.Id == practiceId);
        }

        public void AddPractices(List<int> practiceIdsToAdd)
        {
            practiceIdsToAdd.ForEach(AddPractice);
        }

        private void AddPractice(int practiceId)
        {
            var practice = LoadPractice(practiceId);
            AddPractice(practice);
        }

        private void AddPractice(Practice practice)
        {
            // note the add and delete only works against context but not against list
            Practices.Add(practice);
            var dailyAvailabilitySettings = GetDailyAvailabilitySetting();
            foreach (var dailyAvailabilitySetting in dailyAvailabilitySettings)
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
            var practice = Practices.Find(x => x.Id == practiceId);
            RemovePractice(practice);
        }

        private void RemovePractice(Practice practice)
        {
            var daily = DailyAvailabilities.Where(x => x.PracticeId == practice.Id);
            // Note remove range on the list does not work
            Context.DailyAvailabilities.RemoveRange(daily);
            Practices.Remove(practice);
        }

        private Service LoadService(int serviceId)
        {
            return Context.Services.Find(serviceId);
        }

        public bool ServiceExists(int serviceId)
        {
            return Services.Exists(x => x.Id == serviceId);
        }

        public void AddServices(List<int> serviceIdsToAdd)
        {
            serviceIdsToAdd.ForEach(AddService);
        }

        private void AddService(int serviceId)
        {
            var service = LoadService(serviceId);
            Services.Add(service);
        }

        public void RemoveServices(List<int> serviceIdsToRemove)
        {
            serviceIdsToRemove.ForEach(RemoveService);
        }

        private void RemoveService(int serviceId)
        {
            var service = Services.Find(x => x.Id == serviceId);
            Services.Remove(service);
        }

        private List<DailyAvailabilitySetting> GetDailyAvailabilitySetting()
        {
            return Context.DailyAvailabilitySettings.ToList();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();

            if (Practices.Count == 0)
            {
                results.Add(new ValidationResult("Doctor can not be registered without a practice"));
            }

            return results;
        }

        public static Doctor New(ApplicationDbContext context)
        {
            var entity = new Doctor { Context = context };
            context.Doctors.Add(entity);
            return entity;
        }

        public static Doctor Find(ApplicationDbContext context, int id)
        {
            var entity = context.Doctors.Find(id);
            entity.Context = context;
            return entity;
        }

        public static void Delete(ApplicationDbContext context, int id)
        {
            var entity = context.Doctors.Find(id);
            entity.Context = context;
            entity.IsDeleted = true;
        }

    }

    public class Registration
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string College { get; set; }
    }

    public class Membership
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
    }

    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
        //public bool IsDeleted { get; set; }
    }

    public class Award
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }

    public class Experience
    {
        public int Id { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string As { get; set; }
        public string At { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }

    public class Qualification
    {
        public int Id { get; set; }
        public string College { get; set; }
        public int Year { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }

    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
    }

    public enum FileType
    {
        
        //http://www.mikesdotnetting.com/article/260/mvc-5-with-ef-6-in-visual-basic-working-with-files
        Avatar        
    }

    public class File
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string FileName { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public FileType FileType { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public virtual List<Person> Persons { get; set; }       
    }
        
}


