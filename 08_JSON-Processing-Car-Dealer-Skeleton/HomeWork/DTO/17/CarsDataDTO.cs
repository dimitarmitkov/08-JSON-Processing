using Newtonsoft.Json;

namespace CarDealer.DTO
{
    public class CarsDataDTO
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        [JsonProperty("parts")]
        public PartsOfCarsDTO[] Parts { get; set; }
    }
}
