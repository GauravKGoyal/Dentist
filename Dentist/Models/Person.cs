using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Practice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PracticeTagline { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public virtual List<Person> Persons { get; set; }
        [InverseProperty("Practice")]
        public virtual List<Appointment> PracticeAppointments { get; set; }
        public virtual List<DailyAvailability> DailyAvailabilities{ get; set; }
        public bool IsDeleted { get; set; }
        
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Color { get; set; }
    }

    public class Appointment
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int DoctorId { get; set; }
        [InverseProperty("Appointments")]
        public Person Doctor { get; set; }

        public int PatientId { get; set; }
        [InverseProperty("PatientAppointments")]
        public Person Patient { get; set; }

        public int PracticeId { get; set; }
        [InverseProperty("PracticeAppointments")]
        public Practice Practice { get; set; }

        public string Description { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsBreak { get; set; }
    }

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
        
        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }
    
        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; set; }

        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }

        public virtual List<Practice> Practices { get; set; }

        public bool IsDeleted { get; set; }

        public string Color { get; set; }

    }

    public class DailyAvailability
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        
        public int PracticeId { get; set; }
        public virtual Practice Practice { get; set; }

        public DateTime StartTime1 { get; set; }
        public DateTime EndTime1 { get; set; }
        public DateTime? StartTime2 { get; set; }
        public DateTime? EndTime2 { get; set; }
        public DateTime? StartTime3 { get; set; }
        public DateTime? EndTime3 { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public List<Practice> Practices { get; set; }
    }



}