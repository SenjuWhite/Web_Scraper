using System.ComponentModel.DataAnnotations;
namespace Web_Scraper.Models
{
    public class PhoneInfo
    {
        public int Id { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Screen { get; set; }
        public string? Camera { get; set; } 
        public string? Memory { get; set; }
        public string? Processor { get; set; }
        public string? RAM { get; set; }
        public string? Battery { get; set; }
        public string? Case {get; set; }
        public string? Image { get; set; }

    }
}
