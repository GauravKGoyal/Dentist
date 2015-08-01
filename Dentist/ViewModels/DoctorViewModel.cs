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
    }
}