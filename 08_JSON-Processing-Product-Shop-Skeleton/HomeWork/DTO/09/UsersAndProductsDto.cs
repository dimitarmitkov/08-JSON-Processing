using System;
using System.Collections.Generic;

namespace ProductShop.DTO
{
    public class UsersAndProductsDto
    {
        public UsersAndProductsDto()
        {
            this.Users = new List<UserDto>();
        }

        public int UsersCount { get => this.Users.Count; }

        public List<UserDto> Users { get; set; }
    }
}
