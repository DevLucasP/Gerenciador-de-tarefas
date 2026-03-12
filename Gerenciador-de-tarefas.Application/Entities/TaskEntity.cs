using Gerenciador_de_tarefas.Application.Enums;

namespace Gerenciador_de_tarefas.Application.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate {  get; set; }
        public TaskItemStatus Status { get; set; }
    }
}
