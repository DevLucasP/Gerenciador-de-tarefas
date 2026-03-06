namespace Gerenciador_de_tarefas.Application.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Priority { get; set; }
        public DateTime DuoDate {  get; set; }
        public int Status { get; set; }
    }
}
