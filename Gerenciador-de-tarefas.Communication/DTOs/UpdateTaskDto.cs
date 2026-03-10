namespace Gerenciador_de_tarefas.Communication.Dtos
{
    /// <summary>
    /// Dto utilizado para atualização parcial de uma tarefa.
    /// Apenas os campos enviados serão alterados.
    /// </summary>
    public class UpdateTaskDto
    {
        /// <summary>
        /// Novo nome da tarefa (opcional).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Nova descrição da tarefa (opcional).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Nova prioridade da tarefa (opcional).
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Nova data limite da tarefa (opcional).
        /// Não pode ser no passado. Deve ser uma data futura ou a data atual.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Novo status da tarefa (opcional).
        /// </summary>
        public int? Status { get; set; }
    }
}
