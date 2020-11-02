﻿using System;
using System.ComponentModel.DataAnnotations;
using ToDoApp.ViewModels;

namespace ToDoApp.Models
{
    public class ValidateDateRangeAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"Date must be beyond {DateTime.UtcNow}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var task = (ToDoViewModel)validationContext.ObjectInstance;

            if (task.Deadline < DateTime.UtcNow)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}