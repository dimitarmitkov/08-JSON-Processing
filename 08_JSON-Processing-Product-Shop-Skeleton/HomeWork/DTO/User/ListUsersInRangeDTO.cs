namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class ListUsersInRangeDTO
    {
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ProductSoldListDTO[] ProductsSold { get; set; }
    }
}
