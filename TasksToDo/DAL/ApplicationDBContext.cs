using Microsoft.EntityFrameworkCore;
using TasksToDo.Models;

namespace TasksToDo.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<EmailEntity> EmailsSent { get; set; }
    }
}
