using System;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    public class OrderedCustomersDTO
    {

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BirthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
