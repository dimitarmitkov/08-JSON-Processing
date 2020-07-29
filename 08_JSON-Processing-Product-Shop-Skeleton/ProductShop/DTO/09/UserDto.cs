using System;
namespace ProductShop.DTO
{
    public class UserDto
    {


        string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public SoldProducts SoldProducts { get; set; }
    }



}
