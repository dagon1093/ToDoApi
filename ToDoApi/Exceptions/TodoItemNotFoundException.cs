namespace ToDoApi.Exceptions
{
    public class TodoItemNotFoundException : Exception
    {
        public TodoItemNotFoundException(string message) : base(message) { }
    }
}
