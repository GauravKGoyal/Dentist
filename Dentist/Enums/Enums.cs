using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentist.Enums
{
    public enum AppointmentStatus
    {
        New,
        Confirmed,
        Canceled,
        Met,
        NotMet
    }

    public enum PersonRole
    {
        Patient = 0,
        Doctor = 1
    }

    public enum Title
    {
        Dr,
        Mr,
        Mrs,
        Ms,
        Other
    }

}