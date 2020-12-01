using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoApp.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace ToDoApp.tests.Model
{
    public class DateTimesTestDataGenerator : IEnumerable<object[]>
    {
        private readonly IEnumerable<object[]> _dateTimesToTestCase = new List<object[]>
        {
            new object[]{null, true},
            new object[]{new DateTime(2021,12,12), true},
            new object[]{new DateTime(2020,12,12), true},
            new object[]{new DateTime(2019,12,12), false},
            new object[]{new DateTime(2020,11,10, 20, 28, 0), false}
        };
        public IEnumerator<object[]> GetEnumerator() => _dateTimesToTestCase.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class DeadlineDatetimeValidationShould
    {
        private readonly ITestOutputHelper _output;
        public DeadlineDatetimeValidationShould(ITestOutputHelper output)
        {
            _output = output;
        }
        [Theory]
        [ClassData(typeof(DateTimesTestDataGenerator))]
        public void AcceptEmptyAndFutureDeadline(DateTime? deadline, bool expected)
        {
            var validationResults = new List<ValidationResult>();
            var model = new ToDoViewModel() { Deadline = deadline?.ToUniversalTime(), Task = "Dummy" };
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, validationResults, true);
            Assert.Equal(isValid, expected);
        }
    }
}
