using System.Text.Json;

namespace MVCApi.Models
{
    public class ListTaskTodo
    {
        private static readonly string filePath = "tasks.json";

        public static List<TaskTodo> LoadTasks()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<TaskTodo>>(json) ?? new List<TaskTodo>();
            }
            return new List<TaskTodo>();
        }

        public static void SaveTasks(List<TaskTodo> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
