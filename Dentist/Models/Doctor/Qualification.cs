namespace Dentist.Models.Doctor
{
    public class Qualification
    {
        public int Id { get; set; }
        public string College { get; set; }
        public int Year { get; set; }
        public int DoctorId { get; set; }
        public virtual Models.Doctor.Doctor Doctor { get; set; }
    }
}