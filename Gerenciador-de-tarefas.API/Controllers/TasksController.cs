using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Application.Entities;
using Gerenciador_de_tarefas.Communication.Dtos;
using Gerenciador_de_tarefas.Communication.DTOs;
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
        /// Busca uma tarefa específica pelo identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa.
        /// </param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/tasks/{id}
        ///
        /// Exemplo:
        ///
        ///     GET /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// </remarks>
        /// <returns>
        /// Retorna 200 com os dados da tarefa caso ela exista.
        /// Retorna 404 caso a tarefa não seja encontrada.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Busca a tarefa no banco pelo Id
            var task = await _context.Tasks.FindAsync(id);

            // Caso não exista, retorna 404
            if (task == null)
                return NotFound("Tarefa não encontrada.");

            // Caso exista, retorna 200 com os dados
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

        /// <summary>
        /// Cria uma nova tarefa.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/tasks
        ///     {
        ///         "name": "Estudar ASP.NET",
        ///         "description": "Aprender controllers e EF Core",
        ///         "priority": 1,
        ///         "dueDate": "2026-03-10T00:00:00Z",
        ///         "status": 0
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Retorna 201 quando a tarefa é criada com sucesso.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            // Verifica se o modelo recebido é válido
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validação de regra de negócio:
            // Não permitir tarefas com data no passado
            if (dto.DueDate.Date < DateTime.UtcNow.Date)
                return BadRequest("Erro: a data da tarefa não pode estar no passado.");

            // Cria a entidade Task com base no Dto recebido
            var task = new TaskEntity
            {
                Id = Guid.NewGuid(), // gera um identificador único
                Name = dto.Name,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                Status = dto.Status
            };

            // Adiciona a tarefa ao contexto
            await _context.Tasks.AddAsync(task);

            // Salva no banco de dados
            await _context.SaveChangesAsync();

            // Retorna 201 Created informando que a tarefa foi criada
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        //Próximo passo: criar endpoint para deletar uma tarefa, comentar/testar usando o Swagger e Postman/Insomnia (decidir ainda).
    }
}