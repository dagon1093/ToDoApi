using AutoMapper;
using ToDoApi.Dtos;
using ToDoApi.Models;
using ToDoApi.Repositories;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoServies
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly Mapper _mapper;

        public ToDoService(IToDoRepository toDoRepository, Mapper mapper)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllTodosAsync()
        {
            return await _toDoRepository.GetAllAsync();
        }

        public async Task<ToDoItem> GetTodoByIdAsync(int id)
        {
            return await _toDoRepository.GetByIdAsync(id);
        }

        public async Task<TodoItemDto> CreateTodoAsync(CreateTodoItemDto dto, int userId)
        {
            var todoItem = _mapper.Map<ToDoItem>(dto);
            todoItem.UserId = userId;

            _toDoRepository.AddAsync(todoItem);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task UpdateTodoAsync(int id, ToDoItem todoItem)
        {
            if (id != todoItem.Id)
                throw new Exception("Invalid Id");
            await _toDoRepository.UpdateAsync(todoItem);
        }

        public async Task DeleteTodoAsync(int id)
        {
            await _toDoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ToDoItem>> GetTodosByUserIdAsync(int userId)
        {
            return await _toDoRepository.GetTodosByUserIdAsync(userId);
        }
    }
}
