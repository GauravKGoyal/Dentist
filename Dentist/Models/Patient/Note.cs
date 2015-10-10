using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.Models.Patient
{
    public class Note : IModelWithId
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public int NoteTypeId { get; set; }
        public virtual NoteType NoteType { get; set; }
        [Required]
        public int PatientNoteId { get; set; }
        public virtual PatientNote PatientNote { get; set; }
        public DateTime RecordedDate { get; set; }

    }
}