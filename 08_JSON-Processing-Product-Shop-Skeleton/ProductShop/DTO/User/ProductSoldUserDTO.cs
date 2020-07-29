﻿namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class ProductSoldUserDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
