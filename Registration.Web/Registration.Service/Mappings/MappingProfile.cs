using AutoMapper;
using Registration.Data;
using Registration.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<CustomerDto, Customer>();

            CreateMap<OrderDto, Order>();

            CreateMap<ModuleDto, Order>();

            CreateMap<CustomerCreateDto, Customer>();

        }
    }
}
