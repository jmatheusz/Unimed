using System.Text.Json;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class TaskService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "database.json");

        public List<TaskItem> GetAllTasks()
        {
            if (!File.Exists(_filePath)) return new List<TaskItem>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TaskItem>();
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            File.WriteAllText(_filePath, json);
        }

        public TaskItem AddTask(string title, string priority)
        {
            var tasks = GetAllTasks();
            var newTask = new TaskItem { Title = title, Priority = priority };
            tasks.Add(newTask);
            SaveTasks(tasks);
            return newTask;
        }

        public bool DeleteTask(string id)
        {
            var tasks = GetAllTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            tasks.Remove(task);
            SaveTasks(tasks);
            return true;
        }

        public TaskItem? UpdateTask(string id, bool? completed, string? title, string? priority)
        {
            var tasks = GetAllTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            if (completed.HasValue) task.Completed = completed.Value;
            if (!string.IsNullOrEmpty(title)) task.Title = title;
            if (!string.IsNullOrEmpty(priority)) task.Priority = priority;

            SaveTasks(tasks);
            return task;
        }
    }
}
