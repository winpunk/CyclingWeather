namespace API_Extension.Models
{
    public interface IActivityPossibility
    {
        bool Validate(IWeatherData weather);
    }
}