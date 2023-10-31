using AutoMapper;
using FinanceApp.Models;

namespace FinanceApp.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountCreationViewModel>();
        }
    }
}
