using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Dtos
{
    public class CreateTodoItemDto
    {
        [Required(ErrorMessage = "Заголовок задачи обязателен!")]
        [StringLength(100, ErrorMessage = "Длина заголовка должна быть до 100 символов!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Статус задачи обязателен!")]
        [EnumDataType(typeof(Models.TaskStatus), ErrorMessage = "Некорректный статус задачи!")]
        public Models.TaskStatus Status { get; set; }
    }
}
