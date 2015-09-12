using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models
{
    public class Address
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string AddressLine1 { get; set; }
        [StringLength(100)]
        public string Suburb { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(100)]
        public string PinCode { get; set; }
        public List<Practice> Practices { get; set; }
    }
}