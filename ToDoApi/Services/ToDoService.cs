using ToDoApi.Models;
using ToDoApi.Repositories;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoServies
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoService(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllTodosAsync()
        {
            return await _toDoRepository.GetAllAsync();
        }

        public async Task<ToDoItem> GetTodoByIdAsync(int id)
        {
            return await _toDoRepository.GetByIdAsync(id);
        }

        public async Task<ToDoItem> CreateTodoAsync(ToDoItem todoItem)
        {
            return await _toDoRepository.AddAsync(todoItem);
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
