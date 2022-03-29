using System.ComponentModel.DataAnnotations.Schema;

namespace Evenly.Models
{
#nullable disable
    public class Data
    {
        public int Id { get; set; }
        public int CreatorID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }
        //public string ImagePath { get; set; }
        public long CreatedAt { get; set; }
    }
}
