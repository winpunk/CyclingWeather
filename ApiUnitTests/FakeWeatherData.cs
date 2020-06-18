using API_Extension.Models;


namespace ApiUnitTests
{   
    public class FakeWeatherData : IWeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string _base { get; set; } = "test";
        public Main main { get; set; }
        public int visibility { get; set; } = 0;
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; } = 0;
        public Sys sys { get; set; }
        public int id { get; set; } = 0;
        public string name { get; set; } = "Vilnius";
        public int cod { get; set; } = 200;
    }

    

}
