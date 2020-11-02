using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class ToDoViewModel
    {
        public IEnumerable<ToDoModel> AllTasks { get; set; }
        public int TaskId { get; set; }
        [ValidateDateRange]
        public DateTime? Deadline { get; set; }
        [Required]
        public string Task { get; set; }
        public string User { get; set; }
        public string Notes { get; set; }

    }
}
