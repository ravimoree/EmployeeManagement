using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public static class HireDateValidator
    {
        public static ValidationResult ValidateHireDate(DateTime? date, ValidationContext context)
        {
            if (!date.HasValue)
                return new ValidationResult("Hire Date is required");

            if (date > DateTime.Today)
                return new ValidationResult("Hire Date cannot be a future date");

            return ValidationResult.Success;
        }
    }
}
