using AutoMapper;
using DataAccess.Models;

namespace WebApi.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, Models.Customer>();
            CreateMap<Models.Customer, Customer>();
        }
    }
}
