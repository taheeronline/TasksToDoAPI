using System.ComponentModel.DataAnnotations;

namespace TasksToDo.Models
{
    public class EmailEntity
    {
        [Key]
        public int Id { get; set; }
        public string? EmailFrom { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public DateTime EmailSentDate { get; set; }
    }
}
