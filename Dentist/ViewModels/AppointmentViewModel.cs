using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Enums;

namespace Dentist.ViewModels
{
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
        [UIHint("DoctorSelector")]
        public int DoctorId { get; set; }

        [Display(Name = "Patient")]
        [UIHint("PatientSelector")]
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
}