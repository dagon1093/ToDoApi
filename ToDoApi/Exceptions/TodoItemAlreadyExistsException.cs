namespace ToDoApi.Exceptions
{
    public class TodoItemAlreadyExistsException : Exception
    {
        public TodoItemAlreadyExistsException(string message) : base(message) { }
    }
}
