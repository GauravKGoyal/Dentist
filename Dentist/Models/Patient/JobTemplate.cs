using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class JobTemplate
    {
        public JobTemplate()
        {
            SittingTemplates = new List<SittingTemplate>();
        }
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }

        public virtual List<SittingTemplate> SittingTemplates { get;  private set; }
    }
}