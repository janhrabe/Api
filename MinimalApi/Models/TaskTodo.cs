namespace MinimalApi.Models
{
    public class TaskTodo
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public string? Description { get; set; }
        public Status Status { get; set; }

        //public TaskTodo(string name, DateTime dueDate, string description, Status status)
        //{
        //    Name = name;
        //    DueDate = dueDate;
        //    Description = description;
        //    Status = status;
        //}
    }

}
