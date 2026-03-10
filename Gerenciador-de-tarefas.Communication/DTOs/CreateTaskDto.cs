using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_tarefas.Communication.DTOs
{
    /// <summary>
    /// DTO responsável por representar os dados necessários para criar uma nova tarefa.
    /// </summary>
    /// <remarks>
    /// É utilizado no endpoint de criação de tarefas.
    /// Apenas os campos necessários para o cadastro devem ser informados pelo cliente.
    /// </remarks>
    public class CreateTaskDto
    {
        /// <summary>
        /// Nome da tarefa.
        /// Deve conter pelo menos 3 caracteres.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição opcional da tarefa.
        /// Pode conter até 500 caracteres.
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Prioridade da tarefa.
        /// Valores permitidos:
        /// 1 - Baixa
        /// 2 - Média
        /// 3 - Alta
        /// </summary>
        [Range(1,3)]
        public int Priority { get; set; }

        /// <summary>
        /// Data limite para conclusão da tarefa.
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// Valores permitidos:
        /// 0 - Pendente
        /// 1 - Em andamento
        /// 2 - Concluída
        /// </summary>
        [Range(0,2)]
        public int Status { get; set; }
    }
}
