using Microsoft.AspNetCore.Mvc;
using MVCApi.Models;

namespace MVCApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetEndpointController : ControllerBase
    {
        private static List<TaskTodo> tasks = ListTaskTodo.LoadTasks();


        [HttpGet(Name = "/tasks")]
        public IActionResult GetTasks([FromQuery] string? filter)
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
            return Ok(result);
            //return Results.Json(result);
        }


    }
}
