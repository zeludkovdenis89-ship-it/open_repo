namespace my_first_project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string First_name {get; set;} = string.Empty;
        public string Last_name {get; set;} = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}