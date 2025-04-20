using ToDoApi.Models;

namespace ToDoApi.Services
{
    public interface IToDoServies
    {
        Task<IEnumerable<ToDoItem>> GetAllTodosAsync();
        Task<ToDoItem> GetTodoByIdAsync(int id);
        Task<ToDoItem> CreateTodoAsync(ToDoItem todoItem);
        Task UpdateTodoAsync(int id, ToDoItem todoItem);
        Task DeleteTodoAsync(int id);
        Task<IEnumerable<ToDoItem>> GetTodosByUserIdAsync(int userId);
    }
}
