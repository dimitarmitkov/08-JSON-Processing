using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //14
            this.CreateMap<Customer, OrderedCustomersDTO>();

            //15
            this.CreateMap<Car, ToyotaSelectCarsDTO>();

            //16
            this.CreateMap<Supplier, LocalSuppliersDTO>();

            //17
            this.CreateMap<Part, PartsOfCarsDTO>()
                .ForMember(pcdto => pcdto.Price, opt => opt.MapFrom(p => p.PartCars.Select(pp => pp.Part.Name)))
                .ForMember(pcdto => pcdto.Price, opt => opt.MapFrom(p => p.PartCars.Select(pp => pp.Part.Price)));

            this.CreateMap<Car, CarsDataDTO>()
                .ForMember(cddto => cddto.Parts, opt => opt.MapFrom(c => c.PartCars));

        }
    }
}
