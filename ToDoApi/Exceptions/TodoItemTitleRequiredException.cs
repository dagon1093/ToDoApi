namespace ToDoApi.Exceptions
{
    public class TodoItemTitleRequiredException : Exception
    {
        public TodoItemTitleRequiredException(string message) : base(message) { }
    }
}
