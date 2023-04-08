namespace MVCtoConsumeAPI.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime CompletedDate { get; set; }

        public string CompletionComment { get; set; } = string.Empty;
    }
}
