using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ToDoModel> ToDo { get; set; }
    }
}
