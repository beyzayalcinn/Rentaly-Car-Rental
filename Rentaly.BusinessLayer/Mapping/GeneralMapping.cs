using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rentaly.EntityLayer.Entities;
using AutoMapper;
using Rentaly.DtoLayer.CustomerDtos;

namespace Rentaly.BusinessLayer.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, ResultCustomerDto>().ReverseMap();
            CreateMap<Customer, GetCustomerByIdDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
        }
    }
}
