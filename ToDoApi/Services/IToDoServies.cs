using ToDoApi.Dtos;
using ToDoApi.Models;

namespace ToDoApi.Services
{
    public interface IToDoServies
    {
        Task<IEnumerable<ToDoItem>> GetAllTodosAsync();
        Task<ToDoItem> GetTodoByIdAsync(int id);
        Task<ToDoItem> CreateTodoAsync(CreateTodoItemDto dto, int userId);
        Task UpdateTodoAsync(int id, ToDoItem todoItem);
        Task DeleteTodoAsync(int id);
        Task<IEnumerable<ToDoItem>> GetTodosByUserIdAsync(int userId);
    }
}
