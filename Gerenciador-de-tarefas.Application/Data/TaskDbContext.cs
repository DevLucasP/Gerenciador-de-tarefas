using Gerenciador_de_tarefas.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_tarefas.Application.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }
        public DbSet<TaskEntity> Tasks {  get; set; }
    }
}
