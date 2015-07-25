using System.Collections.Generic;
using Dentist.Helpers;

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

    }
}