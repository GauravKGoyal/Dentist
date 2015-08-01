using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Paitient : Person
    {
        public Paitient() : base()
        {
            PersonRole = PersonRole.Patient;
        }

        [NotMapped]
        public ApplicationDbContext Context { get; set; }

        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }

        public virtual Practice Practice { get; set; }
        public int PracticeId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();

            if (Practice == null && PracticeId == 0)
            {
                results.Add(new ValidationResult("Paitient has to be registered with a practice"));
            }
            return results;
        }

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