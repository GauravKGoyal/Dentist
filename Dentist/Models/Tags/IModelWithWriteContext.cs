using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models.Tags
{
    public interface IModelWithWriteContext
    {
        [NotMapped]
        WriteContext Context { get; set; }
    }
}
