using Dentist.Models.Patient;
using Dentist.Models.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dentist.Models
{
    public class Procedure : IModelWithId, IModelWithName
    {
        public int Id { get; set; }

        [Required, Index(IsUnique = true), StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public double Cost { get; set; }

        public virtual List<Treatment> Treatments { get; set; }
    }
}