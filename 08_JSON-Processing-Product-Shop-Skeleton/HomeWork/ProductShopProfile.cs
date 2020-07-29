using System.Linq;
using AutoMapper;
using ProductShop.DTO.Product;
using ProductShop.DTO.User;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //06
            this.CreateMap<Product, ListProductsInRangeDTO>()
                .ForMember(lpir => lpir.SellerName, opt => opt.MapFrom(p => p.Seller.FirstName + " " + p.Seller.LastName));

            //07
            this.CreateMap<Product, ProductSoldListDTO>()
                .ForMember(psl => psl.BuyerFirtsName, opt => opt.MapFrom(p => p.Buyer.FirstName))
                .ForMember(psl => psl.BuyerLastName, opt => opt.MapFrom(p => p.Buyer.LastName));

            this.CreateMap<User, ListUsersInRangeDTO>()
                .ForMember(luir => luir.ProductsSold, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.Buyer != null)));

            //08:
            this.CreateMap<User, UserCountProductSold>()
                .ForMember(ucps => ucps.UsersCount, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.Buyer != null).Count()))
            .ForMember(ucps => ucps.UsersDTO, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.Buyer != null)));

            this.CreateMap<UsersDTO, UserCountProductSold>();
            //    .ForMember(ucps => ucps.UsersCount, opt => opt.MapFrom(udto => udto.LastName.Count()));



            this.CreateMap<User, UsersDTO>()
            //.ForMember(udto => udto.LastName, opt => opt.MapFrom(u => u.LastName))
            //.ForMember(udto => udto.Age, opt => opt.MapFrom(u => u.Age));
            .ForMember(udto => udto.SoldProducts, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.Buyer != null)))
            .ForMember(udto => udto.Count, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.Buyer != null).Count()));

            //this.CreateMap<UsersDTO, ProductSoldUserCountDTO>()
            //    .ForMember(psucdto => psucdto.Count, opt => opt.MapFrom(u => u.SoldProducts.Count()));

            this.CreateMap<Product, ProductSoldUserDTO>();

        }
    }
}
