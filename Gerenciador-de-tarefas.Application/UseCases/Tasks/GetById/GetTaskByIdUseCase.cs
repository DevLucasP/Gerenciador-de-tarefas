using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Communication.Responses;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_tarefas.Application.UseCases.Tasks.GetById
{
    /// <summary>
    /// Caso de uso responsável por buscar uma tarefa pelo identificador.
    /// </summary>
    public class GetTaskByIdUseCase
    {
        private readonly TaskDbContext _context;

        /// <summary>
        /// Inicializa o caso de uso.
        /// </summary>
        public GetTaskByIdUseCase(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca uma tarefa pelo id.
        /// </summary>
        /// <param name="id">Identificador único da tarefa.</param>
        /// <returns>Dados da tarefa encontrada.</returns>
        public async Task<ResponseTaskJson?> Execute(Guid id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return null;

            return new ResponseTaskJson
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                Status = task.Status
            };
        }
    }
}