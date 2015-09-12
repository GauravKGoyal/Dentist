using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class Note
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public int NoteTypeId { get; set; }
        public virtual NoteType NoteType { get; set; }
        [Required]
        public int PatientId { get; set; }
        public virtual Paitient Patient { get; set; }
    }
}