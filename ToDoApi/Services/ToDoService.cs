using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ToDoApi.Dtos;
using ToDoApi.Models;
using ToDoApi.Repositories;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoServies
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ToDoService(IToDoRepository toDoRepository, IMapper mapper, IMemoryCache cache)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
            _cache = cache;
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

            await _toDoRepository.AddAsync(todoItem);

            _cache.Remove($"UserTodos_{userId}");

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

        public async Task<PagedResult<TodoItemDto>> GetTodosByUserIdAsync(int userId, int page, int pagesize, int? status, string sortBy, string order)
        {

            Models.TaskStatus? statusEnum = null;

            if (status != null && Enum.IsDefined(typeof(Models.TaskStatus), status.Value))
            {
                statusEnum = (Models.TaskStatus)status.Value;
            }

            var cacheKeyTotalCount = $"UserCountTodos_{userId}_{statusEnum}";

            if(_cache.TryGetValue(cacheKeyTotalCount, out int cachedUserTodosCount))
            {
                var totalCount = await _toDoRepository.GetUserTodosCountAsync(userId, statusEnum);

                cachedUserTodosCount = totalCount;

                _cache.Set(cacheKeyTotalCount, cachedUserTodosCount, TimeSpan.FromMinutes(5));
            }


            var cacheKey = $"UserTodos_{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<ToDoItem> cachedTodos)) 
            {
                cachedTodos = await _toDoRepository.GetAllUserTodosAsync(userId);

                _cache.Set(cacheKey, cachedTodos, TimeSpan.FromMinutes(5));
            }

            var todos = cachedTodos.AsQueryable();

            if (status.HasValue)
            {
                todos = todos.Where(t => t.Status == (Models.TaskStatus)status.Value);
            }

            todos = (sortBy?.ToLower(), order?.ToLower()) switch
            {
                ("title", "asc") => todos.OrderBy(t => t.Title),
                ("title", "desc") => todos.OrderByDescending(t => t.Title),
                ("createdat", "asc") => todos.OrderBy(t => t.CreatedAt),
                _ => todos.OrderByDescending(t => t.CreatedAt)
            };


            var totalPages = (int)Math.Ceiling(cachedUserTodosCount / (double)pagesize);

            todos = todos.Skip((page - 1) * pagesize).Take(pagesize);

            var mappedTodos = _mapper.Map<IEnumerable<TodoItemDto>>(todos).ToList();

            return new PagedResult<TodoItemDto>
            {
                Page = page,
                PageSize = pagesize,
                TotalCount = cachedUserTodosCount,
                TotalPages = totalPages,
                Items = mappedTodos
            };
        }
    }
}
