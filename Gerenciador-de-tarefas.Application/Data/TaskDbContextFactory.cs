using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gerenciador_de_tarefas.Application.Data;

public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=gerenciador_tarefas;Username=postgres;Password=root"
        );

        return new TaskDbContext(optionsBuilder.Options);
    }
}