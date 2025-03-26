namespace MinimalApi.Models
{
    public class CreateTaskTodo
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public string? Description { get; set; }

        public CreateTaskTodo(string name, DateTime dueDate, string description)
        {
            Name = name;
            DueDate = dueDate;
            Description = description;
        }
    }
}
