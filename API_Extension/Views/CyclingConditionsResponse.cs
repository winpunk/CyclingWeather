using API_Extension.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace API_Extension.Views
{
    public class CyclingConditionsResponse : IResponse
    {
        public ActionResult<CyclingConditions> Create(bool isCyclable, IWeatherData weather)
        {
            return new CyclingConditions()
            {
                isItCyclable = isCyclable,
                temperature = weather.main.temp,
                windSpeed = weather.wind.speed,
                windDirectionDeg = weather.wind.deg,
                weatherCondition = weather.weather.FirstOrDefault().main,
                lastUpdated = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
            };
        }
    }
}