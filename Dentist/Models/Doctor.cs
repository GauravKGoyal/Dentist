using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using System.Linq;
using WebGrease.Css.Extensions;

namespace Dentist.Models
{
    public class Doctor : Person
    {
        public Doctor()
        {
            PersonRole = Enums.PersonRole.Doctor;
            Practices = new List<Practice>();
            DailyAvailabilities = new List<DailyAvailability>();
            Appointments = new List<Appointment>();
        }

        public ApplicationDbContext Context { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }

        public virtual List<Practice> Practices { get; set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; private set; }

        public string Color { get; set; }

        public bool PracticeExists(int practiceId)
        {
            return Practices.Exists(x => x.Id == practiceId);
        }

        public void AddPractices(List<int> practiceIdsToAdd)
        {
            practiceIdsToAdd.ForEach(AddPractice);
        }

        public void AddPractice(int practiceId)
        {
            var practice = LoadPractice(practiceId);
            AddPractice(practice);
        }

        public void AddPractice(Practice practice)
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

        public void RemovePractice(int practiceId)
        {
            var practice = Practices.Find(x => x.Id == practiceId);
            RemovePractice(practice);
        }

        public void RemovePractice(Practice practice)
        {
            var daily = DailyAvailabilities.Where(x => x.PracticeId == practice.Id);
            // Note remove range on the list does not work
            Context.DailyAvailabilities.RemoveRange(daily);
            Practices.Remove(practice);
        }

        public void RemovePractices(List<int> practiceIdsToRemove)
        {
            practiceIdsToRemove.ForEach(RemovePractice);
        }

        public void RemovePractices(List<Practice> practicesToRemove)
        {
            practicesToRemove.ForEach(RemovePractice);                     
        }

        private Practice LoadPractice(int practiceId)
        {
            return Context.Practices.Find(practiceId);
        }

        private List<DailyAvailabilitySetting> GetDailyAvailabilitySetting()
        {
            return Context.DailyAvailabilitySettings.ToList();
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
}

