using System.ComponentModel.DataAnnotations;

namespace Dentist.Models
{
    public class BasePersistentModel
    {
        [Editable(false)]
        public int Id { get; set; }
    }
}
