using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dentist.Enums;
using Dentist.Models.Tags;

namespace Dentist.Models
{
    public class Person : IValidatableObject, IModelWithIsDelete
    {
        public Person()
            : base()
        {
            Address = new Address();
        }

        public int Id { get; set; }

        public virtual List<File> Files { get; set; }

        public Title Title { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string Phone { get; set; }

        public PersonRole PersonRole { get; protected set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public bool IsDeleted { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Files != null && Files.Count > 0)
            {
                Files.ForEach(f =>
                {
                    if (f.Content.Length > (1000*1000))
                    {
                        result.Add(new ValidationResult("File cannot be bigger than 1 Mb"));
                    }
                });
            }
            return result;
        }
    }
}