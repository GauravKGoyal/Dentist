using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Paitient : Person
    {
        public Paitient()
        {
            PersonRole = PersonRole.Patient;
        }

        public ApplicationDbContext Context { get; set; }

        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }

        public virtual Practice Practice { get; set; }
        public int PracticeId { get; set; }

        public static Paitient New(ApplicationDbContext context)
        {
            var entity = new Paitient { Context = context };
            context.Paitients.Add(entity);
            return entity;
        }

        public static Paitient Find(ApplicationDbContext context, int id)
        {
            var entity = context.Paitients.Find(id);
            entity.Context = context;
            return entity;
        }

        public static void Delete(ApplicationDbContext context, int id)
        {
            var entity = context.Paitients.Find(id);
            entity.Context = context;
            entity.IsDeleted = true;
        }
    }
}