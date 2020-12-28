
namespace GoEatapp_backend.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string MailIndex { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string CuisineNationality { get; set; }
        public string Interior { get; set; }
        public string Tagline { get; set; }
        public string Other { get; set; }
    }

    public class PlaceCreate
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string MailIndex { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public int CuisineNationality { get; set; }
        public int Interior { get; set; }
        public string Tagline { get; set; }
        public string Other { get; set; }
    }
}
