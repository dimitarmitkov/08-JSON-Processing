namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class UserCountProductSold
    {
        [JsonProperty("count")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public UsersDTO[] UsersDTO { get; set; }
    }
}
