using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Communication.Requests;

namespace Gerenciador_de_tarefas.Application.UseCases.Tasks.Update
{
    /// <summary>
    /// Caso de uso responsável por atualizar uma tarefa existente.
    /// </summary>
    public class UpdateTaskUseCase
    {
        private readonly TaskDbContext _context;

        public UpdateTaskUseCase(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executa a atualização de uma tarefa.
        /// </summary>
        /// <param name="id">Identificador da tarefa.</param>
        /// <param name="request">Dados atualizados da tarefa.</param>
        /// <exception cref="ArgumentException">
        /// Lançada quando a data informada está no passado.
        /// </exception>
        public async Task Execute(Guid id, UpdateTaskRequest request)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            if (request.DueDate.HasValue &&
                request.DueDate.Value.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("A data não pode estar no passado.");
            }

            if (request.Name != null)
                task.Name = request.Name;

            if (request.Description != null)
                task.Description = request.Description;

            if (request.Priority.HasValue)
                task.Priority = request.Priority.Value;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate.Value;

            if (request.Status.HasValue)
                task.Status = request.Status.Value;

            await _context.SaveChangesAsync();
        }
    }
}