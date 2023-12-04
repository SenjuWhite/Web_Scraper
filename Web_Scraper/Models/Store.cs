using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Web_Scraper.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public string PhoneId { get; set; }
        public Phone Phone { get; set; }
        public string? PhoneLink { get; set; }
        public string? Price { get; set; }
        public string? StoreName { get; set; }

    }
}
