using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dentist.Helpers
{
    public static class YearHelper
    {
        public static IEnumerable<SelectListItem> GetSelectList()
        {
            var list = new List<SelectListItem>();
            for (int i = 1920; i <= DateTime.Today.Year; i++)
            {
               list.Add(new SelectListItem(){Text=Convert.ToString(i), Value = Convert.ToString(i)}); 
            }
            
           return list;
        }
    }
}
