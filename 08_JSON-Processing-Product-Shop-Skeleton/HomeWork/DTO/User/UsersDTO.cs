namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class UsersDTO
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("soldProducts")]
        public ProductSoldUserDTO[] SoldProducts { get; set; }
    }
}
