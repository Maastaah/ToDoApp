using System;
using System.ComponentModel.DataAnnotations;
using ToDoApp.ViewModels;

namespace ToDoApp.Models
{
    public sealed class ValidateDateRangeAttribute : ValidationAttribute
    {
        public static string GetErrorMessage() => $"Date must be beyond {DateTime.UtcNow}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var task = (ToDoViewModel)validationContext.ObjectInstance;

            if (task.Deadline.HasValue)
            {
                DateTime convert = (DateTime)task.Deadline;
                DateTime convertedDeadline = DateTime.SpecifyKind(convert, DateTimeKind.Utc);
                if (convertedDeadline < DateTime.UtcNow)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
}