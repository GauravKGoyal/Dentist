using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;

namespace Dentist.ViewModels
{
    public class PracticeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name="Practice Tag Line")]
        public string PracticeTagline { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int AddressId { get; set; }
        public AddressView Address { get; set; }
        public string Color { get; set; }
    }

    public class PersonView
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Role")]
        public PersonRole PersonRole { get; set; }

        public int AddressId { get; set; }

        public AddressView Address { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsDoctor
        {
            get { return PersonRole == PersonRole.Doctor; }
        }

        public string Color { get; set; }

        [RequiredListItem]
        public List<int> Practices { get; set; }

    }

    public class DoctorView
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int AddressId { get; set; }

        public AddressView Address { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsDoctor
        {
            get { return true; }
        }

        public string Color { get; set; }

        [RequiredListItem]
        public List<int> Practices { get; set; }

    }

    public class PatientView
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int AddressId { get; set; }

        public AddressView Address { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name="Practice")]
        public int PatientViewPracticeId { get; set; }

        public bool IsDoctor
        {
            get { return false; }
        }

    }

    public class DoctorListView
    {
        public int Id { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class PatientListView
    {
        public int Id { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class PersonListView
    {

        public int Id { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Role")]
        public PersonRole PersonRole { get; set; }

        public int AddressId { get; set; }

        public virtual AddressView Address { get; set; }

        public string Color { get; set; }
    }

    public class DailyAvailabilityView
    {
        public int Id { get; set; }
        [Display(Name = "Day")]
        public DayOfWeek DayOfWeek { get; set; }
        [Display(Name = "Person")]
        public int DailyAvailabilityViewPersonId { get; set; }
        [Display(Name = "Practice")]
        public int DailyAvailabilityViewPracticeId { get; set; }
        [Display(Name = "Practice")]
        public string DailyAvailabilityViewPracticeName { get; set; }
        [Display(Name = "Start1")]
        public DateTime? StartTime1 { get; set; }
        [Display(Name = "End1")]
        public DateTime? EndTime1 { get; set; }
        [Display(Name = "Start2")]
        public DateTime? StartTime2 { get; set; }
        [Display(Name = "End2")]
        public DateTime? EndTime2 { get; set; }
        [Display(Name = "Start3")]
        public DateTime? StartTime3 { get; set; }
        [Display(Name = "End3")]
        public DateTime? EndTime3 { get; set; }
        public bool IsWorking { get; set; }
    }

    public class AppointmentView
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start")]
        public DateTime StartDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "End")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Display(Name = "Practice")]
        public int PracticeId { get; set; }
        public string PracticeName { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Display(Name = "First Name")]
        public string DoctorFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string DoctorLastName { get; set; }

        [Display(Name = "Phone")]
        public string DoctorPhone { get; set; }

        [Display(Name = "Email")]
        public string DoctorEmail { get; set; }

        [Display(Name = "First Name")]
        public string PatientFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string PatientLastName { get; set; }

        [Display(Name = "Phone")]
        public string PatientPhone { get; set; }

        [Display(Name = "Email")]
        public string PatientEmail { get; set; }

    }

    public class AddressView
    {
        public int Id { get; set; }
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
    }

}