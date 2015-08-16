namespace Dentist.Models.Doctor
{
    public class Experience
    {
        public int Id { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string As { get; set; }
        public string At { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}