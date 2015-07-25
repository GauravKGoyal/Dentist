using System.Collections.Generic;

namespace Dentist.Models
{
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