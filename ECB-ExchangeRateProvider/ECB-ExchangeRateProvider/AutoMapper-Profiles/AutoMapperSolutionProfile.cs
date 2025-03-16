using AutoMapper;
using ECB_ExchangeRateProvider.DTO;
using ECB_ExchangeRateProvider.Models;

namespace ECB_ExchangeRateProvider.AutoMapper_Profiles {
    public class AutoMapperSolutionProfile : Profile {
        public AutoMapperSolutionProfile() {
            CreateMap<ExchangeRateModel, ExchangeRateDto>().ReverseMap();
            CreateMap<WalletModel, WalletDto>().ReverseMap();
        }
    }
}
