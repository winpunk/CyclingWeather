namespace API_Extension.Models
{
    public class CyclingConditions
    {
        public bool isItCyclable { get; set; }
        public float temperature { get; set; }
        public float windSpeed { get; set; }
        public int windDirectionDeg { get; set; }
        public string weatherCondition { get; set; }
        public string lastUpdated { get; set; }
    }
}