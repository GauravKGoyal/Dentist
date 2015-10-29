using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Profile;
using Dentist.Models.Tags;

namespace Dentist.Models.Patient
{
    /// <summary>
    /// Patient Note is envelop of notes for a patient
    /// </summary>
    public class PatientNote : IModelWithId
    {
        public PatientNote()
        {
            Notes = new List<Note>(); //owner (M:1) 
        }

        public int Id { get; set; }
        public virtual List<Note> Notes { get; private set; }
        public virtual Patient Patient { get; set; }
        public int PatientId { get; set; }
        public DateTime RecordedDate { get; set; }

    }
}
