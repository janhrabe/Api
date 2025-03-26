using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using MinimalApi.Models;
namespace MinimalApi
{
    public class Program
    {
        private static List<TaskTodo> tasks;
        private static readonly string filePath = "tasks.json";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var app = builder.Build();


            tasks = LoadTasks();

            app.MapGet("/tasks", (string? filter) =>
            {
                List<TaskTodo> result;

                switch (filter)
                {
                    case "past":
                    result = tasks.Where(t => t.DueDate < DateTime.Now).ToList();
                    break;
                    case "next":
                    result = tasks.Where(t => t.DueDate >= DateTime.Now)
                                  .OrderBy(t => t.DueDate)
                                  .Take(1)
                                  .ToList();
                    break;
                    case "today":
                    result = tasks.Where(t => t.DueDate.Date == DateTime.Today).ToList();
                    break;
                    case "thisweek":
                    var today = DateTime.Today;
                    var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                    var endOfWeek = startOfWeek.AddDays(6);
                    result = tasks.Where(t => t.DueDate >= startOfWeek && t.DueDate <= endOfWeek).ToList();
                    break;
                    case "new":
                    result = tasks.Where(t => t.Status == Status.New).ToList();
                    break;
                    case "active":
                    result = tasks.Where(t => t.Status == Status.Active).ToList();
                    break;
                    case "done":
                    result = tasks.Where(t => t.Status == Status.Done).ToList();
                    break;
                    case "all":
                    result = tasks;
                    break;
                    default:
                    result = tasks.Where(t => t.DueDate >= DateTime.Now).ToList();
                    break;
                }

                return Results.Json(result);
            });

            app.MapPut("/tasks/{id}", (Guid Id, EditTaskTodo updatedTask) =>
                {
                    var task = tasks.FirstOrDefault(t => t.Id == Id);
                    if (task == null)
                    {
                        return Results.BadRequest("Task not found");
                    }
                    task.Name = updatedTask.Name;
                    task.Description = updatedTask.Description;
                    task.Status = updatedTask.Status;

                    SaveTasks(tasks);
                    return Results.Ok(task);
                });

            app.MapPost("/tasks", (CreateTaskTodo newTask) =>
            {
                if (string.IsNullOrEmpty(newTask.Name))
                {
                    return Results.BadRequest("Task name is required.");
                }

                if (newTask.DueDate < DateTime.Now)
                {
                    return Results.BadRequest("Due date must be in the future.");
                }

                var task = new TaskTodo
                {
                    Id = Guid.NewGuid(),
                    Name = newTask.Name,
                    DueDate = newTask.DueDate,
                    Description = newTask.Description,
                    Status = Status.New,
                };

                tasks.Add(task);
                SaveTasks(tasks);

                return Results.Ok(newTask);
            });

            app.MapDelete("/tasks/{id}", (Guid id) =>
            {
                var task = tasks.Find(t => t.Id == id);
                if (task.Status != Status.New)
                {
                    return Results.BadRequest("Removing Tasks with different status than New is not allowed");
                }
                tasks.Remove(task);
                SaveTasks(tasks);
                return Results.Ok();
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
        private static List<TaskTodo> LoadTasks()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<TaskTodo>>(json) ?? new List<TaskTodo>();
            }
            return new List<TaskTodo>();
        }

        private static void SaveTasks(List<TaskTodo> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
