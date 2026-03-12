using Gerenciador_de_tarefas.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_tarefas.Communication.Requests
{
    /// <summary>
    /// Dados necessários para criar uma tarefa.
    /// </summary>
    public class CreateTaskRequest
    {
        /// <summary>
        /// Nome da tarefa.
        /// </summary>
        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve possuir pelo menos 3 caracteres.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da tarefa.
        /// </summary>
        [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        /// <summary>
        /// Prioridade da tarefa.
        /// </summary>
        [Required]
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Data limite para conclusão da tarefa.
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// </summary>
        public TaskItemStatus Status { get; set; }
    }
}