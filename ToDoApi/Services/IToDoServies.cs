using ToDoApi.Dtos;
using ToDoApi.Models;

namespace ToDoApi.Services
{
    public interface IToDoServies
    {
        Task<IEnumerable<ToDoItem>> GetAllTodosAsync();
        Task<ToDoItem> GetTodoByIdAsync(int id);
        Task<TodoItemDto> CreateTodoAsync(CreateTodoItemDto dto, int userId);
        Task UpdateTodoAsync(int id, ToDoItem todoItem);
        Task DeleteTodoAsync(int id);
        Task<PagedResult<TodoItemDto>> GetTodosByUserIdAsync(int userId, int page, int pagesize, int? status, string sortBy, string order);
    }
}
