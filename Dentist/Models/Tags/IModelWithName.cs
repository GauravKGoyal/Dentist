using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Tags
{
    public interface IModelWithName
    {
        [Required]
        string Name { get; set; }
    }
}
