﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Web;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;

namespace Dentist.ViewModels
{
    public class PracticeViewModel
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

    public class PersonViewModel
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
    }
    
    public class DoctorViewModel : PersonViewModel
    {
        public bool IsDoctor
        {
            get { return true; }
        }

        public string Color { get; set; }

        [RequiredListItem]
        public List<int> Practices { get; set; }

    }

    public class PatientViewModel : PersonViewModel
    {
        [Display(Name="Practice")]
        public int PatientViewPracticeId { get; set; }

        public bool IsDoctor
        {
            get { return false; }
        }

    }

    public class DoctorListViewModelModel : PersonListViewModel
    {
        
    }

    public class PatientListViewModelModel : PersonListViewModel
    {
        
    }

    public class PersonListViewModel
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

    public class DailyAvailabilityViewModel
    {
        private DateTime? _startTime1;
        private DateTime? _endTime1;
        private DateTime? _startTime2;
        private DateTime? _endTime2;
        private DateTime? _startTime3;
        private DateTime? _endTime3;

        public int Id { get; set; }
        [Display(Name = "Day")]
        public DayOfWeek DayOfWeek { get; set; }
        [Display(Name = "Person")]
        public int DailyAvailabilityViewModelPersonId { get; set; }
        [Display(Name = "Practice")]
        public int DailyAvailabilityViewModelPracticeId { get; set; }
        [Display(Name = "Practice")]
        public string DailyAvailabilityViewModelPracticeName { get; set; }

        [Display(Name = "Start1")]
        public DateTime? StartTime1
        {
            get
            {
                return IsWorking ? _startTime1 : null;
            }
            set
            {
                _startTime1 = value;                
            }
        }

        [Display(Name = "End1")]
        public DateTime? EndTime1
        {
            get
            {
                return IsWorking ? _endTime1 : null;
            }
            set
            {
                _endTime1 = value;
            }
        }

        [Display(Name = "Start2")]
        public DateTime? StartTime2
        {
            get
            {
                return IsWorking ? _startTime2 : null;
            }
            set
            {
                _startTime2 = value;
            }
        }

        [Display(Name = "End2")]
        public DateTime? EndTime2
        {
            get
            {
                return IsWorking ? _endTime2 : null;
            }
            set
            {
                _endTime2 = value;
            }
        }

        [Display(Name = "Start3")]
        public DateTime? StartTime3
        {
            get
            {
                return IsWorking ? _startTime3 : null;
            }
            set
            {
                _startTime3 = value;
            }
        }

        [Display(Name = "End3")]
        public DateTime? EndTime3
        {
            get
            {
                return IsWorking ? _endTime3 : null;
            }
            set
            {
                _endTime3 = value;
            }
        }

        public bool IsWorking { get; set; }
    }

    public class AppointmentViewModel
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