using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentist.Helpers
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _defaultErrorMessage;
        private readonly string _propertyNameToPropertyToBeGreater;
        private readonly string _propertyNameToComparewith;

        public GreaterThanAttribute(string propertyToBeGreater, string propertyToCompareWith, string message)
        {
            _propertyNameToPropertyToBeGreater = propertyToBeGreater;
            _propertyNameToComparewith = propertyToCompareWith;
            _defaultErrorMessage = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = ValidationResult.Success;
            
            var propertyInfoToCompare = validationContext.ObjectType.GetProperty(_propertyNameToPropertyToBeGreater);

            var propertyInfoToCompareWith = validationContext.ObjectType.GetProperty(_propertyNameToComparewith);
            var propertyValueToCompareWith = propertyInfoToCompareWith.GetValue(validationContext.ObjectInstance, null);
            
            if (propertyValueToCompareWith == null)
            {
                return new ValidationResult("property Value To Compare With cannot be empty");
            }

            if (value == null)
            {
                return new ValidationResult("property Value To Compare cannot be empty");
            }

            if (propertyInfoToCompare.PropertyType == typeof(DateTime))
            {
                var valueToCompare = (DateTime)value;
                var valueToCompareWith = Convert.ToDateTime(propertyValueToCompareWith);
                if (DateTime.Compare(valueToCompare, valueToCompareWith) <= 0)
                {
                    validationResult = new ValidationResult(_defaultErrorMessage);
                }
            }
            else if (propertyInfoToCompare.PropertyType == typeof(int))
            {
                var valueToCompare = Convert.ToInt32(value);
                var valueToCompareWith = Convert.ToInt32(propertyValueToCompareWith);
                if (valueToCompare <= valueToCompareWith)
                {
                    validationResult = new ValidationResult(_defaultErrorMessage);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return validationResult;
        }
    }
}
