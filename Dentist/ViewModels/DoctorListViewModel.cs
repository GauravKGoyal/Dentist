namespace Dentist.ViewModels
{
    public class DoctorListViewModel : PersonListViewModel
    {
        public string About { get; set; }
        public string QualificationsName { get; set; }
        public int? ExperienceInYears { get; set; }
    }
}