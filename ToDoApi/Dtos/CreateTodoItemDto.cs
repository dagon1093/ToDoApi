namespace ToDoApi.Dtos
{
    public class CreateTodoItemDto
    {
        public string Title { get; set; }
        public Models.TaskStatus Status { get; set; }
    }
}
