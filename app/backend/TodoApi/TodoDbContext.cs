using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options): base(options)
    {
    }

    public DbSet<ToDo> ToDos => Set<ToDo>();
}