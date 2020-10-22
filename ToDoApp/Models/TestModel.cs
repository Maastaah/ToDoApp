using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Task { get; set; }
        public string Notes { get; set; }
        public DateTime Deadline { get; set; }
    }
}
