namespace API_Extension.Models
{
    public class Cyclability : IActivityPossibility
    {
        public bool Validate(IWeatherData weather)
        {
            if (weather.main.temp < 10 || weather.main.temp > 30 || weather.wind.speed > 7 || weather.weather[0].id < 700)
            {
                return false;
            }
            else return true;
        }
    }
}