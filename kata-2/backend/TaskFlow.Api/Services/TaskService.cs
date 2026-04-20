using System.Text.Json;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class TaskService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "database.json");
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            await _lock.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return new List<TaskItem>();
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<TaskItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TaskItem>();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SaveTasksAsync(List<TaskItem> tasks)
        {
            await _lock.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<TaskItem> AddTaskAsync(string title, string priority)
        {
            var tasks = await GetAllTasksAsync();
            var newTask = new TaskItem { Title = title, Priority = priority };
            tasks.Add(newTask);
            await SaveTasksAsync(tasks);
            return newTask;
        }

        public async Task<bool> DeleteTaskAsync(string id)
        {
            var tasks = await GetAllTasksAsync();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            tasks.Remove(task);
            await SaveTasksAsync(tasks);
            return true;
        }

        public async Task<TaskItem?> UpdateTaskAsync(string id, bool? completed, string? title, string? priority)
        {
            var tasks = await GetAllTasksAsync();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return null;

            if (completed.HasValue) task.Completed = completed.Value;
            if (!string.IsNullOrEmpty(title)) task.Title = title;
            if (!string.IsNullOrEmpty(priority)) task.Priority = priority;

            await SaveTasksAsync(tasks);
            return task;
        }
    }
}
