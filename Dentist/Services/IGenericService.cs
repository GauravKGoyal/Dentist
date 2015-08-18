using Dentist.Models;
using Dentist.Models.Tags;

namespace Dentist.Services
{
    public interface IGenericService<T> where T : class, IModelWithWriteContext, IModelWithIsDelete
    {
        T New(WriteContext context);
        T Find(WriteContext context, int id);
        bool TryDeleteAndCommit(WriteContext context, int id, out string errorMessage);
    }
}