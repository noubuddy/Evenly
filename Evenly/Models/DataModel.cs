namespace Evenly.Models
{
    public class DataModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Coordinates { get; set; }
        public DateTime Time { get; set; } = new DateTime();
    }
}
