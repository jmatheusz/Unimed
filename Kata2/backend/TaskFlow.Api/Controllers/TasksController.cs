using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController()
        {
            _taskService = new TaskService(); // Simples para este Kata, idealmente via DI
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? status)
        {
            var tasks = _taskService.GetAllTasks();
            if (status == "pending") tasks = tasks.Where(t => !t.Completed).ToList();
            else if (status == "completed") tasks = tasks.Where(t => t.Completed).ToList();
            
            return Ok(tasks);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskRequest request)
        {
            if (string.IsNullOrEmpty(request.Title)) return BadRequest("Title is required");
            var task = _taskService.AddTask(request.Title, request.Priority ?? "Média");
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] TaskUpdateRequest request)
        {
            var task = _taskService.UpdateTask(id, request.Completed, request.Title, request.Priority);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_taskService.DeleteTask(id)) return NoContent();
            return NotFound();
        }
    }

    public class TaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Priority { get; set; }
    }

    public class TaskUpdateRequest
    {
        public bool? Completed { get; set; }
        public string? Title { get; set; }
        public string? Priority { get; set; }
    }
}
