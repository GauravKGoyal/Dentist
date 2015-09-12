using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Tags
{
    public interface IModelWithName
    {
        [Required]
        [StringLength(100)]
        string Name { get; set; }
    }
}
