using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dentist.Helpers
{
    public class RequiredListItemAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Value cannot be empty");
            }

            if ((value.GetType().IsGenericType) && (value is IEnumerable<int>))
            {
                var valueToValidate = (IEnumerable<int>) value;
                if (!valueToValidate.Any())
                {
                    return new ValidationResult("Value cannot be empty");
                }
            }
            else
            {
                throw new Exception(string.Format("Object type is not supported for validation [{0}]",
                    validationContext.ObjectType.ToString()));
            }

            return ValidationResult.Success;
        }
    }
}