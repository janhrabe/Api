namespace MinimalApi.Models
{
    public record EditTaskTodo
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public string? Description { get; set; }
        public Status Status { get; set; }

        public EditTaskTodo(string name, DateTime dueDate, string description, Status status)
        {
            Name = name;
            DueDate = dueDate;
            Description = description;
            Status = status;
        }
    }
}
