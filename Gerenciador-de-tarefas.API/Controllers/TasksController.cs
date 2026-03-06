using Gerenciador_de_tarefas.Application.Data;
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
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task <IActionResult> GetAll()
        {
            var task = await _context.Tasks.ToListAsync();
            if (task.Count == 0)
                return NoContent();
            return Ok(task);
        }
        
        //Proximo passo: Criar demais endpoints
    }
}
