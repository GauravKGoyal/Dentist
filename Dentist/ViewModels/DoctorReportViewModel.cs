using Dentist.Models;
using Dentist.Models.Doctor;
using System.Collections.Generic;
using System.Linq;

namespace Dentist.ViewModels
{
    public class DoctorReportViewModel
    {
        private readonly Doctor doctor;
        private readonly ApplicationDbContext context;
        private readonly int totalServicesCount;
        private const string delimiter = ",";

        public DoctorReportViewModel(Doctor doctor, ApplicationDbContext context)
        {
            this.doctor = doctor;
            this.context = context;
            this.totalServicesCount = this.doctor.Services.Count;
        }
        public string Name => $"Dr {this.doctor.FirstName}  {this.doctor.LastName}";
        public string Qualifications => this.doctor.Qualifications.Any() ? this.doctor.Qualifications.Select((q) => q.Name).Aggregate((i, j) => i + delimiter + j) : "";
        public string Specializations => this.doctor.Specializations.Any() ? this.doctor.Specializations.Select((q) => q.Name).Aggregate((i, j) => i + delimiter + j) : "";
        public string Experience => this.doctor.ExperienceInYears.HasValue ? $"{this.doctor.ExperienceInYears.ToString()} years experience" : "";
        public string About => this.doctor.About;
        public IEnumerable<PracticeViewModel> Practices => this.doctor.Practices.Select((p) => new PracticeViewModel(p));

        public IEnumerable<string> ServicesGroup1 => this.doctor.Services.Select((s) => s.Name).OrderBy(s => s).Take(this.totalServicesCount / 2);
        public IEnumerable<string> ServicesGroup2 => this.doctor.Services.Select((s) => s.Name).OrderBy(s => s).Skip(this.totalServicesCount / 2);
        public int AvatarId => context.Files
                    .Where(f => f.FileType == FileType.Avatar)
                    .Where(f => f.Persons.Any(p => p.Id == this.doctor.Id))
                    .OrderByDescending(f => f.Id)
                    .Select(f => f.Id)
                    .FirstOrDefault();

        public class PracticeViewModel
        {
            public PracticeViewModel(Practice practice)
            {
                this.practice = practice;
            }

            private Practice practice { get; }

            public string Suburb => this.practice.Address?.Suburb;
            public string PracticeName => this.practice.Name;
            public string Address => this.practice.Address?.AddressLine1;
        }
    }

}