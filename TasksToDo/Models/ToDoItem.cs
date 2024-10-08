﻿using System.ComponentModel.DataAnnotations;

namespace TasksToDo.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; }= DateTime.Now;

        public DateTime CompletedDate { get; set;}

        public string CompletionComment { get; set; } = string.Empty;
    }
}
