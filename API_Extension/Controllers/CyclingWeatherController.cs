using API_Extension.Models;
using API_Extension.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace API_Extension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CyclingWeatherController : ControllerBase
    {
        private IResponse _cyclingConditionsResponse;
        private IActivityPossibility _activityPossibility;
        private IApi _weatherApi;

        public CyclingWeatherController(IResponse jsonResponse, IActivityPossibility activityPossibility, IApi weatherApi)
        {
            _cyclingConditionsResponse = jsonResponse;
            _activityPossibility = activityPossibility;
            _weatherApi = weatherApi;
        }

        [HttpGet]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "city" })]
        [Produces("application/json")]
        public async Task<ActionResult<CyclingConditions>> GetCyclingWeatherStatus(string city)
        {
            bool isCyclable = false;
            IWeatherData weather;

            if (city == null) return BadRequest();

            try
            {
                weather = await _weatherApi.getData(city);

                isCyclable = _activityPossibility.Validate(weather);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, "Internal server Error");
            }

            return _cyclingConditionsResponse.Create(isCyclable, weather);
        }
    }
}