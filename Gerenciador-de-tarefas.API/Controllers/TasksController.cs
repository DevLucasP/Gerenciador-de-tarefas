using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Communication.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_tarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context) => _context = context;

        /// <summary>
        /// Lista todas as tarefas 
        /// </summary>
        /// <returns>
        /// Retorna uma lista de tarefas ou nenhum conteúdo se não houver registros.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var task = await _context.Tasks.ToListAsync();
            if (task.Count == 0)
                return NoContent();
            return Ok(task);
        }

        /// <summary>
        /// Atualiza parcialmente os dados de uma tarefa.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa.
        /// </param>
        /// <param name="dto">
        /// Objeto contendo os campos que serão atualizados.
        /// Apenas os campos enviados na requisição serão modificados.
        /// </param>
        /// <remarks>
        /// É possível atualizar somente os campos desejados da tarefa.
        ///
        /// Exemplo 1 - Atualizar somente o nome:
        ///
        ///     PUT /api/tasks/{id}
        ///     {
        ///         "name": "Estudar ASP.NET Core"
        ///     }
        ///
        /// Exemplo 2 - Atualizar prioridade e status:
        ///
        ///     PUT /api/tasks/{id}
        ///     {
        ///         "priority": 3,
        ///         "status": 2
        ///     }
        ///
        /// Exemplo 3 - Atualizar descrição e data limite:
        ///
        ///     PUT /api/tasks/{id}
        ///     {
        ///         "description": "Finalizar projeto da Rocketseat",
        ///         "dueDate": "2026-03-15T18:00:00Z"
        ///     }
        ///
        /// Apenas os campos enviados serão atualizados.
        /// Campos omitidos permanecerão com seus valores atuais.
        /// </remarks>
        /// <returns>
        /// Retorna 204 (No Content) se a atualização for realizada com sucesso.
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto dto)
        {
            //Buscar a tarefa pelo id e verificar se existe
            var task = await _context.Tasks.FindAsync(id);

            //Se não existir, retornar NotFound
            if (task == null)
                return NotFound("Tarefa não encontrada");

            // Atualiza o nome, somente se for enviado no request
            if (dto.Name != null)
            {
                //Validação do nome
                if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 100)
                    return BadRequest("Nome inválido: o nome da tarefa deve conter entre 1 e 100 caracteres.");

                //Remove os espaços em branco do início e do fim do nome e atualiza
                task.Name = dto.Name.Trim();
            }

            // Atualiza a descrição somente se foi enviada
            if (dto.Description != null)
            {
                task.Description = dto.Description;
            }

            //Atualiza a prioridade somente se veio no dto
            if (dto.Priority.HasValue)
            {
                task.Priority = dto.Priority.Value;
            }

            // Atualiza a data limite 
            if (dto.DueDate.HasValue)
            {
                //A data nao pode estar no passado.
                if (dto.DueDate.Value < DateTime.UtcNow)
                    return BadRequest("Erro: a data limite não pode estar no passado.");

                task.DueDate = dto.DueDate.Value;
            }

            //Atualiza o status
            if (dto.Status.HasValue)
            {
                task.Status = dto.Status.Value;
            }

            //Salva alterações no banco de dados
            await _context.SaveChangesAsync();

            //retorna 204, indicando que a atualização foi bem-sucedida.
            return NoContent();
        }

        //Próximo passo: criar endpoint para deletar uma tarefa e outro para criar uma nova tarefa, comentar e testar usando o Swagger
    }
}