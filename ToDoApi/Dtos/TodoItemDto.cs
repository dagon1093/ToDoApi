using ToDoApi.Models;

namespace ToDoApi.Dtos
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Models.TaskStatus Status { get; set; }
    }
}
