﻿using ToDoApi.Models;

namespace ToDoApi.Repositories
{
    public interface IToDoRepository
    {
        Task <IEnumerable<ToDoItem>> GetAllAsync();
        Task<ToDoItem> GetByIdAsync(int id);
        Task<ToDoItem> AddAsync(ToDoItem todoItem);
        Task UpdateAsync(ToDoItem todoItem);
        Task DeleteAsync(int id);
        Task<IEnumerable<ToDoItem>> GetTodosByUserIdAsync(int userId);
        Task<List<ToDoItem>> GetAllUserTodosAsync(int userId);
        Task<int> GetUserTodosCountAsync(int userId, Models.TaskStatus? status);
    }
}
