using Gerenciador_de_tarefas.Application.Data;
using Gerenciador_de_tarefas.Application.UseCases.Tasks.Create;
using Gerenciador_de_tarefas.Application.UseCases.Tasks.Delete;
using Gerenciador_de_tarefas.Application.UseCases.Tasks.GetAll;
using Gerenciador_de_tarefas.Application.UseCases.Tasks.GetById;
using Gerenciador_de_tarefas.Application.UseCases.Tasks.Update;
using Gerenciador_de_tarefas.Communication.Requests;
using Gerenciador_de_tarefas.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador_de_tarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context) => _context = context;

        /// <summary>
        /// Lista todas as tarefas.
        /// </summary>
        /// <returns>
        /// Retorna 200 com a lista de tarefas.
        /// Retorna 204 caso não existam registros.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseTaskJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var useCase = new GetAllTasksUseCase(_context);

            var tasks = await useCase.Execute();

            if (tasks.Count == 0)
                return NoContent();

            return Ok(tasks);
        }

        /// <summary>
        /// Busca uma tarefa pelo identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa.
        /// </param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/tasks/{id}
        ///
        /// </remarks>
        /// <returns>
        /// Retorna 200 com os dados da tarefa caso ela exista.
        /// Retorna 404 caso não seja encontrada.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseTaskJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var useCase = new GetTaskByIdUseCase(_context);

            var result = await useCase.Execute(id);

            if (result == null)
                return NotFound("Tarefa não encontrada.");

            return Ok(result);
        }

        /// <summary>
        /// Atualiza parcialmente os dados de uma tarefa.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa.
        /// </param>
        /// <param name="request">
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
        ///         "priority": "High",
        ///         "status": "InProgress"
        ///     }
        ///
        /// Exemplo 3 - Atualizar descrição e data limite:
        ///
        ///     PUT /api/tasks/{id}
        ///     {
        ///         "description": "Finalizar projeto",
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
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskRequest request)
        {
            var useCase = new UpdateTaskUseCase(_context);

            await useCase.Execute(id, request);

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
        ///         "priority": "Medium",
        ///         "dueDate": "2026-03-10T00:00:00Z",
        ///         "status": "Pending"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">
        /// Dados necessários para criação da tarefa.
        /// </param>
        /// <returns>
        /// Retorna 201 Created quando a tarefa é criada com sucesso.
        /// Retorna 400 caso os dados informados sejam inválidos.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            // Validação automática do modelo (DataAnnotations)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var useCase = new CreateTaskUseCase(_context);

            try
            {
                var id = await useCase.Execute(request);

                return CreatedAtAction(nameof(GetById), new { id }, request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma tarefa existente.
        /// </summary>
        /// <param name="id">
        /// Identificador único (GUID) da tarefa a ser removida.
        /// </param>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/tasks/{id}
        ///
        /// Exemplo:
        ///
        ///     DELETE /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// </remarks>
        /// <returns>
        /// Retorna 204 se a tarefa for removida com sucesso.
        /// Retorna 404 caso a tarefa não seja encontrada.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var useCase = new DeleteTaskByIdUseCase(_context);

            var deleted = await useCase.Execute(id);

            if (!deleted)
                return NotFound("Tarefa não encontrada.");

            return NoContent();
        }

        /*Próximos passos: 
         * 1) terminar a refatoração;
         * 2) Conferir se comentários xml estão bem fundamentados; e
         * 3) Testar usando o Postman.
         */
    }
}