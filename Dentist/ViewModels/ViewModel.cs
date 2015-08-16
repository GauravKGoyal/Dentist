using System;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using Dentist.Models;

namespace Dentist.ViewModels
{
    public class FakeViewModel : ModelWithName  
    {
        public string String     { get; set; }
        public DateTime DateTime { get; set; }
        public bool Boolean { get; set; }
    }
}