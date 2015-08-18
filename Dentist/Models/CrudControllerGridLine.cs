using Dentist.Models.Tags;

namespace Dentist.Models
{
    public class CrudControllerGridLine : IModelWithId, IModelWithName 
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
