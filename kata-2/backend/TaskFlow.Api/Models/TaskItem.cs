namespace TaskFlow.Api.Models
{
    public class TaskItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public string Priority { get; set; } = "Média";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
