﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoApi.Data;
using ToDoApi.Dtos;
using ToDoApi.Exceptions;
using ToDoApi.Models;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoServies _toDoService;
        private readonly IMapper _mapper;

        public ToDoController(IToDoServies toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        [HttpGet("all")]
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

            var dto = _mapper.Map<TodoItemDto>(todoItem);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateToDoItem(CreateTodoItemDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


          var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var createdItem = await _toDoService.CreateTodoAsync(dto, userId);
            return CreatedAtAction(nameof(GetToDoItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem(int id, ToDoItem todoItem)
        {
            if (id != todoItem.Id)
                return BadRequest();

            await _toDoService.UpdateTodoAsync(id, todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var todoItem = await _toDoService.GetTodoByIdAsync(id);
            if (todoItem == null)
                return NotFound();
            await _toDoService.DeleteTodoAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetUserTodos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? status = null,
            [FromQuery] string sortBy = "createdAt",
            [FromQuery] string order = "desc")
            
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var todos = await _toDoService.GetTodosByUserIdAsync(userId, page, pageSize, status, sortBy, order);
            return Ok(todos);
        }


    }
}
