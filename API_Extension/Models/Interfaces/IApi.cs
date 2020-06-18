using System.Threading.Tasks;

namespace API_Extension.Models
{
    public interface IApi
    {
        Task<WeatherData> getData(string parameter);
    }
}