using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Tags
{
    public interface IModelWithId
    {
        [Editable(false)]
        int Id { get; set; }
    }
}
