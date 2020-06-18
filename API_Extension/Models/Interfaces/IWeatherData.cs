namespace API_Extension.Models
{
    public interface IWeatherData
    {
        Coord coord { get; set; }
        Weather[] weather { get; set; }
        string _base { get; set; }
        Main main { get; set; }
        int visibility { get; set; }
        Wind wind { get; set; }
        Clouds clouds { get; set; }
        int dt { get; set; }
        Sys sys { get; set; }
        int id { get; set; }
        string name { get; set; }
        int cod { get; set; }
    }
}