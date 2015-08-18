using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dentist.Models;
using Dentist.Models.Doctor;

namespace Dentist.Services
{
    public class DoctorService : GenericService<Doctor>, IDoctorService
    {
        public override bool TryDeleteAndCommit(WriteContext context, int id, out string errorMessage)
        {
            Doctor entity = context
                .Doctors
                .Include(x => x.Practices)
                .First(x => x.Id == id);

            entity.Context = context;
            entity.IsDeleted = true;
            return context.TrySaveChanges(out errorMessage);
        }
    }
}
