using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dentist.Enums;
using Dentist.Models.Tags;

namespace Dentist.Models
{
    public class Paitient : Person
    {
        public Paitient() : base()
        {
            PersonRole = PersonRole.Patient;
        }

        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }

        public virtual Practice Practice { get; set; }
        public int PracticeId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();

            if (Practice == null && PracticeId == 0)
            {
                results.Add(new ValidationResult("Paitient can not be registered without a practice"));
            }
            return results;
        }
    }
}