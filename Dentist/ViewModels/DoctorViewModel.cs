using System.Collections.Generic;
using AutoMapper;
using Dentist.Helpers;
using Dentist.Models;
using System.Linq;

namespace Dentist.ViewModels
{
    public class DoctorViewModel : PersonViewModel
    {
        public bool IsDoctor
        {
            get { return true; }
        }

        public string Color { get; set; }

        [RequiredListItem]
        public List<int> Practices { get; set; }

        public List<int> PracticeIdsToRemove(Doctor doctor)
        {
            return doctor.Practices.Where(practice => !Practices.Contains(practice.Id)).Select(x => x.Id).ToList();
        }

        public List<int> PracticeIdsToAdd(Doctor doctor)
        {
            return Practices.Where(practiceId => doctor.Practices.All(practice => practice.Id != practiceId))
                    .ToList();    
        }

        public List<int> Services { get; set; }

        public List<int> ServiceIdsToRemove(Doctor doctor)
        {
            return doctor.Services.Where(service => !Services.Contains(service.Id)).Select(x => x.Id).ToList();
        }

        public List<int> ServiceIdsToAdd(Doctor doctor)
        {           
            return Services.Where(serviceId => doctor.Services.All(service => service.Id != serviceId))
                    .ToList();
        }

        public void CopyTo(Doctor doctor)
        {
             base.CopyTo(doctor);
             Mapper.Map(this, doctor);

             if (UploadedAvatarFile != null)
             { 
                doctor.Files.Add(UploadedAvatarFile);
             }
        }

        public void CopyFrom(Doctor doctor)
        {
            base.CopyFrom(doctor);
            Mapper.Map(doctor, this);
        }
    }
}