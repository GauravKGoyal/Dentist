using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models
{
    public class File
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string FileName { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public FileType FileType { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public virtual List<Person> Persons { get; set; }       
    }
}