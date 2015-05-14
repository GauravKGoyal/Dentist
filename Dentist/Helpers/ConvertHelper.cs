using System;
using System.Collections.Generic;
using System.Linq;

namespace Dentist.Helpers
{
    public static class ConvertHelper
    {
        public static IList<int> ToInts(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<int>();
            }

            var tempValues = value.Split(',');
            var result = tempValues.Select(x => Convert.ToInt32(x));
            return result.ToList();
        }
    }
}