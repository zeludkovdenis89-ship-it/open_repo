namespace my_first_project.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string title { get; set; } = string.Empty;
        public string description {get; set;} = string.Empty;
    }
}