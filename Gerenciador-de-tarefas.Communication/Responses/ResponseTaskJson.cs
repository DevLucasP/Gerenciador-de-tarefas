using Gerenciador_de_tarefas.Application.Enums;

namespace Gerenciador_de_tarefas.Communication.Responses
{
    /// <summary>
    /// Representa os dados retornados de uma tarefa.
    /// </summary>
    public class ResponseTaskJson
    {
        /// <summary>
        /// Identificador único da tarefa.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome da tarefa.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da tarefa.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Prioridade da tarefa.
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Data limite para conclusão da tarefa.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// </summary>
        public TaskItemStatus Status { get; set; }
    }
}