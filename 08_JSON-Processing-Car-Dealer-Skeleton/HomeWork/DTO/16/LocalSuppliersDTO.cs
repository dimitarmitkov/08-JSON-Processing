using System;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    public class LocalSuppliersDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("PartsCount")]
        public int PartsCount { get; set; }
    }
}
