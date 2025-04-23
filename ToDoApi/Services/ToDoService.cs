using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Dtos;
using ToDoApi.Models;
using ToDoApi.Repositories;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoServies
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IMapper _mapper;

        public ToDoService(IToDoRepository toDoRepository, IMapper mapper)
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

            await _toDoRepository.AddAsync(todoItem);
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
        public async Task<PagedResult<TodoItemDto>> GetTodosByUserIdAsync(int userId, int page, int pagesize, int? status)
        {
            Models.TaskStatus? statusEnum = null;

            if (status != null && Enum.IsDefined(typeof(Models.TaskStatus), status.Value))
            {
                statusEnum = (Models.TaskStatus)status.Value;
            }

            var totalCount = await _toDoRepository.GetUserTodosCountAsync(userId, statusEnum);
            var items = await _toDoRepository.GetUserTodosAsync(userId, page, pagesize, statusEnum);
            var dtoItems = _mapper.Map<List<TodoItemDto>>(items);   

            var totalPages = (int)Math.Ceiling(totalCount / (double)pagesize);

            return new PagedResult<TodoItemDto>
            {
                Page = page,
                PageSize = pagesize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = dtoItems
            };
        }
    }
}
