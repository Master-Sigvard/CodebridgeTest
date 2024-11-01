using System.ComponentModel.DataAnnotations;

namespace CodebridgeTest.Models
{
    public class Dogs
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TailLength { get; set; }
        public int Weight { get; set; }
    }
}