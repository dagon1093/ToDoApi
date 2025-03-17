using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Exceptions;
using ToDoApi.Models;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoServies _toDoService;

        public ToDoController(IToDoServies toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTodoItems()
        {
            var todos = await _toDoService.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
        {
            var todoItem = await _toDoService.GetTodoByIdAsync(id);
            if (todoItem == null)    
                return NotFound(new {message = "ToDoItem not found"}); 
            
            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> CreateToDoItem(ToDoItem todoItem)
        {
            var todo = await _toDoService.CreateTodoAsync(todoItem);

            return CreatedAtAction(nameof(GetToDoItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem(int id, ToDoItem todoItem)
        {
            if (id != todoItem.Id)
                return BadRequest();

            _toDoService.UpdateTodoAsync(id, todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var todoItem = await _toDoService.GetTodoByIdAsync(id);
            if (todoItem == null)
                return NotFound();
            _toDoService.DeleteTodoAsync(id);
            return NoContent();
        }


    }
}
