namespace Gerenciador_de_tarefas.Communication.Dtos
{
    public class UpdateTaskDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Priority { get; set; }

        public DateTime? DueDate { get; set; }
        public int? Status { get; set; }
    }
}
