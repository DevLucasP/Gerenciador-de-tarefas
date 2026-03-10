using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gerenciador_de_tarefas.Communication.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;
        [Range(1,3)]
        public int Priority { get; set; }
        public DateTime DueDate { get; set; }
        [Range(0,2)]
        public int Status { get; set; }
    }
}
