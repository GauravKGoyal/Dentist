using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AutoMapper;

namespace Dentist.Models.Doctor
{
    public class Qualification : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string College { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public virtual Models.Doctor.Doctor Doctor { get; set; }
        public override bool Equals(object obj)
        {
            var qualification = (Qualification) obj;
            return this.Name == qualification.Name && 
                this.College == qualification.College && 
                this.Year == qualification.Year;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.College.GetHashCode() ^ this.Year.GetHashCode();
        }

        [NotMapped]
        public WriteContext Context { get; set; }

        public bool HasDuplicateQualification()
        {
            // if distinct qualifications are same as non distinct qualification that means there is no repeated qualification with same name, college, year
            // Note that distinct make use of Equals method to get unique names
            return this.Doctor.Qualifications.Distinct().Count() != this.Doctor.Qualifications.Count();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Context == null)
            {
                throw new Exception("Context missing in Qualification");
            }

            var results = new List<ValidationResult>();

            // Lazy loading is turned off by EF during validation therefore load the doctor manually
            if (!Context.Entry(this).Reference(p => p.Doctor).IsLoaded)
            {
                Context.Entry(this).Reference(p => p.Doctor).Load();
                this.Doctor.Context = Context;
            }
            if (this.Doctor.Qualifications.Count > 1)
            {
                if (HasDuplicateQualification())
                {
                    results.Add(new ValidationResult("Qualification cannot be repeated"));
                }
            }

            return results;
        }
    }
}