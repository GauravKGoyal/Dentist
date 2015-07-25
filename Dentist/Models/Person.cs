﻿using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Person
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Phone { get; set; }

        public PersonRole PersonRole { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public bool IsDeleted { get; set; }
    }
}