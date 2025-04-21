using ToDoApi.Models;

namespace ToDoApi.Dtos
{
    public class TodoDtoMapper
    {
        public static TodoItemDto ToDto(ToDoItem item)
        {
            return new TodoItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Status = item.Status
            };
        }
    }
}
