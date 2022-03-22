namespace Evenly.Models
{
    public class Data
    {
        public int Id { get; set; }
        public int CreatorID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Coordinates { get; set; }
        public string? Image { get; set; }
        public DateTime Time { get; set; }
    }
}
