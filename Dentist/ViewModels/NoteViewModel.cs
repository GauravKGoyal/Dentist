using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.ViewModels
{
    public class NoteViewModel : IModelWithId 
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [UIHint("NoteTypeSelector")]
        [Display(Name ="Note type")]
        public int NoteTypeId { get; set; }
        [Display(Name = "-")]
        public string NoteTypeDescription { get; set; }
        
        [Required]
        public int PatientNoteId { get; set; }
        
    }

}
