namespace Web_Scraper.Models
{
    public class Data
    {
        public List<Store> Store { get; set; }
        public List<Phone> Phone { get; set; }
        public Data()
        {
                Store = new List<Store>();
                Phone = new List<Phone>();
        }
    }
}
