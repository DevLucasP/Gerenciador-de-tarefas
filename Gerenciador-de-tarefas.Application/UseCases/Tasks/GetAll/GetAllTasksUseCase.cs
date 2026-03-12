using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Communication.Responses;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_tarefas.Application.UseCases.Tasks.GetAll
{
    /// <summary>
    /// Caso de uso responsável por listar todas as tarefas.
    /// </summary>
    public class GetAllTasksUseCase
    {
        private readonly TaskDbContext _context;

        public GetAllTasksUseCase(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as tarefas cadastradas.
        /// </summary>
        public async Task<List<ResponseTaskJson>> Execute()
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .ToListAsync();

            return tasks.Select(task => new ResponseTaskJson
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                Status = task.Status
            }).ToList();
        }
    }
}