using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dentist.Enums;

namespace Dentist.Models.Patient
{
    public class Paitient : Person
    {
        public Paitient() : base()
        {
            PersonRole = PersonRole.Patient;
            VitalSigns = new List<VitalSign>(); //owner (M:1)
            TreatmentPlans = new List<TreatmentPlan>(); //owner (M:1)
            Notes = new List<Note>(); //owner (M:1) 
        }

        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }
        public virtual List<VitalSign> VitalSigns { get; private set; }
        public virtual List<TreatmentPlan> TreatmentPlans { get; private set; }
        public virtual List<Note> Notes { get; private set; }

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