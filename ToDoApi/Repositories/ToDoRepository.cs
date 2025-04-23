using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Dtos;
using ToDoApi.Exceptions;
using ToDoApi.Models;

namespace ToDoApi.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ToDoRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<ToDoItem> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<ToDoItem> AddAsync(ToDoItem todoItem)
        {
            if (_context.TodoItems.Any(t => t.Title == todoItem.Title))
            {
                throw new TodoItemAlreadyExistsException("ToDoItem with the same title already exists");
            }
            if (string.IsNullOrEmpty(todoItem.Title))
            {
                throw new TodoItemTitleRequiredException("Title is required");
            }
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task UpdateAsync(ToDoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                throw new TodoItemNotFoundException("ToDoItem not found");
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToDoItem>> GetTodosByUserIdAsync(int userId)
        {
            return await _context.TodoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ToDoItem>> GetUserTodosAsync(int userId, int page, int pageSize, Models.TaskStatus? status)
        {
            var query = _context.TodoItems.Where(t => t.UserId == userId);

            if (status != null)
            {
                query = query.Where(t => t.Status == status);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUserTodosCountAsync(int userId, Models.TaskStatus? status)
        {
            var query = _context.TodoItems.Where(t => t.UserId == userId);

            if (status != null)
            {
                query = query.Where(t => t.Status == status);
            }

            return await query.CountAsync();

        }

        }
}
