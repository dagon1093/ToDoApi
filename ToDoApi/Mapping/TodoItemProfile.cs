using AutoMapper;
using ToDoApi.Dtos;
using ToDoApi.Models;

namespace ToDoApi.Mapping
{
    public class TodoItemProfile: Profile
    {
        public TodoItemProfile()
        {
            CreateMap<ToDoItem, TodoItemDto>();
            CreateMap<TodoItemDto, ToDoItem>();

        }
    }
}
