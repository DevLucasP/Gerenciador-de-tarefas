using Gerenciador_de_tarefas.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_tarefas.Application.UseCases.Tasks.Delete
{
    /// <summary>
    /// Caso de uso responsável por remover uma tarefa do sistema.
    /// </summary>
    /// <remarks>
    /// Este UseCase busca uma tarefa pelo identificador informado.
    /// Caso a tarefa exista, ela é removida do banco de dados.
    /// </remarks>
    public class DeleteTaskByIdUseCase
    {
        private readonly TaskDbContext _context;
        /// <summary>
        /// Inicializa uma nova instância do caso de uso de remoção de tarefa.
        /// </summary>
        /// <param name="context">
        /// Contexto de acesso ao banco de dados utilizado para manipular as entidades.
        /// </param>
        public DeleteTaskByIdUseCase(TaskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Executa a remoção de uma tarefa.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa que será removida.
        /// </param>
        /// <returns>
        /// Retorna <c>true</c> se a tarefa foi encontrada e removida.
        /// Retorna <c>false</c> caso a tarefa não exista.
        /// </returns>
        public async Task<bool> Execute(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}