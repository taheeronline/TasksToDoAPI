﻿namespace MVCtoConsumeAPI.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
