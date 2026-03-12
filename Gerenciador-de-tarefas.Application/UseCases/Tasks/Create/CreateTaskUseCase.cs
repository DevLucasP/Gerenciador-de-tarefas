using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Application.Entities;
using Gerenciador_de_tarefas.Communication.Requests;

namespace Gerenciador_de_tarefas.Application.UseCases.Tasks.Create
{
    /// <summary>
    /// Caso de uso responsável por criar uma nova tarefa.
    /// </summary>
    public class CreateTaskUseCase
    {
        private readonly TaskDbContext _context;

        public CreateTaskUseCase(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executa a criação de uma tarefa.
        /// </summary>
        /// <param name="request">Dados da nova tarefa.</param>
        /// <returns>Identificador da tarefa criada.</returns>
        /// <exception cref="ArgumentException">
        /// Lançada quando a data informada está no passado.
        /// </exception>
        public async Task<Guid> Execute(CreateTaskRequest request)
        {
            // Regra de negócio
            if (request.DueDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Erro: a data da tarefa não pode estar no passado.");

            var task = new TaskEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Priority = request.Priority,
                DueDate = request.DueDate,
                Status = request.Status
            };

            await _context.Tasks.AddAsync(task);

            await _context.SaveChangesAsync();

            return task.Id;
        }
    }
}