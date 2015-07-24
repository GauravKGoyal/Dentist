using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Dentist.Enums;
using Dentist.Models;
using Kendo.Mvc.UI;
using Microsoft.Owin.Security.Google;

namespace Dentist.ViewModels
{

    public class SchedulerPracticeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class SchedulerDoctorView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
        //public List<SchedulerPracticeView> Practices { get; set; }
    }

    public class SchedulerAppointmentView : ISchedulerEvent
    {
        public int Id { get; set; }

        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        [Display(Name = "Search")]
        public int? PatientId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Status")]
        public AppointmentStatus AppointmentStatus { get; set; }
        
        [Display(Name = "Practice")]
        public int PracticeId { get; set; }

        public string PracticeColor { get; set; }

        //Inherited
        public DateTime Start { get; set; }
        //Inherited
        public DateTime End { get; set; }
        //Inherited 
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        //Inherited
        private string _title;
        public string Title
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {
                _title = value;
            }
        }

        //Inherited
        [Display(Name = "Recurrence")]
        public string RecurrenceRule { get; set; }
        //Inherited
        public string RecurrenceException { get; set; }

        [Display(Name = "Is Break")]
        public bool IsBreak { get; set; }

        //Inherited Exclude
        public bool IsAllDay { get; set; }
        //Inherited  Exclude        
        public string StartTimezone { get; set; }
        //Inherited Exclude
        public string EndTimezone { get; set; }

    }
}

