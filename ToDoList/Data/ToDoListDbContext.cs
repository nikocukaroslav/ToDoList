using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public class ToDoListDbContext : DbContext
{
    public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDo { get; set; }
    public DbSet<Category> Category { get; set; }
}