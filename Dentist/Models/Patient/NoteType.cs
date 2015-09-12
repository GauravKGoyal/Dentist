using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models.Patient
{
    public class NoteType
    {
        public NoteType()
        {
            Notes = new List<Note>();
        }
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Description { get; set; }
        public virtual List<Note> Notes { get; private set; }
    }
}