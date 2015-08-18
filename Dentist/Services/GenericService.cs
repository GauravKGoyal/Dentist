using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.Models.Tags;

namespace Dentist.Services
{
    public class GenericService<T> : IGenericService<T> where T:class, IModelWithWriteContext, IModelWithIsDelete
    {
        protected GenericService()
        {
            
        }
        public virtual T New(WriteContext context)
        {
            var model = (T)Activator.CreateInstance(typeof(T));
            model.Context = context;
            context.Set<T>().Add(model);
            return model;
        }

        public virtual T Find(WriteContext context, int id)
        {
            var model = context.Set<T>().Find(id);
            model.Context = context;
            return model;
        }

        public virtual bool TryDeleteAndCommit(WriteContext context, int id, out string errorMessage)
        {
            var model = context.Set<T>().Find(id);
            model.Context = context;
            model.IsDeleted = true;
            return context.TrySaveChanges(out errorMessage);
        }
    }
}
