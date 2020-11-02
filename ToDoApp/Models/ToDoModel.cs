﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDoModel
    {
        [Key]
        public int Id { get; set; }
        public string User { get; set; }
        [Required]
        public string Task { get; set; }
        public string Notes { get; set; }
        public bool IsDone { get; set; }
        [ValidateDateRange]
        public DateTime? Deadline { get; set; }

    }
}
