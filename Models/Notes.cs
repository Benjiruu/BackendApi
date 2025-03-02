namespace Backend_Api.Models
{
    public class Note
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public bool IsDone { get; set; }
    }
}
