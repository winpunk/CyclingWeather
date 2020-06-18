using API_Extension.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Extension.Views
{
    public interface IResponse
    {
        ActionResult<CyclingConditions> Create(bool isCyclable, IWeatherData weather);
    }
}