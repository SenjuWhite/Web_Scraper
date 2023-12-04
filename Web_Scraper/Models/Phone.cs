using System.ComponentModel.DataAnnotations;
namespace Web_Scraper.Models
{
    public class Phone
    {
        
        public string Id { get; set; }
        public string? Model { get; set; }

        public string? Brand { get; set; }
        public string? Year { get; set; }
        public string? Screen { get; set; }
        public string? Size { get; set; } = "unknown";
        public string? Refresh_Rate { get; set; } = "unknown";


        public string? Camera { get; set; }
        public string? MainCameraMP { get; set; } = "unknown";
        public string? FrontCameraMP { get; set; } = "unknown";

        public string? Memory { get; set; }
        public string? Processor { get; set; }
        public string? RAM { get; set; }
        public string? Battery { get; set; }
        public string? Case {get; set; }
        public string? Image { get; set; }

    }
}
