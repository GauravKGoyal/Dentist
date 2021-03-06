﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.ViewModels
{    
    public class PatientNoteViewModel : IModelWithId
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime RecordedDate { get; set; }

        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        public string PatientFullName
        {
            get
            {
                return PatientFirstName + " " + PatientLastName;
            }
        }

    }

    public class PatientNoteDto : IValidatableObject
    {
        public PatientNoteDto()
        {
            Notes = new List<NoteDto>();
        }
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime RecordedDate { get; set; }
        public List<NoteDto> Notes { get; set; }

        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFullName
        {
            get
            {
                return PatientFirstName + " " + PatientLastName;
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (PatientId == 0)
            {
                results.Add(new ValidationResult("Patient Id cannot be 0", new List<string> {"PatientId"}));
            }

            if ((Notes == null) || ( Notes.Count == 0))
            {
                results.Add(new ValidationResult("Notes cannot be null", new List<string> { "Notes" }));            
            }

            //if ((Notes != null) || (Notes.Count > 0))
            //{
            //    var anyNoteIsEmpty = Notes.TrueForAll(note => !string.IsNullOrWhiteSpace(note.Description));
            //    if (anyNoteIsEmpty)
            //    {
            //        results.Add(new ValidationResult("Any of the notes cannot be empty", new List<string> { "Notes" }));
            //    }
            //}

            return results;
        }
    }

    public class NoteDto
    {
        public int Id { get; set; }
        public int PatientNoteDtoId { get; set; }
        public int NoteTypeId { get; set; }
        [Required]
        public string Description { get; set; }
        public string ObjectState { get; set; }
    }
}
