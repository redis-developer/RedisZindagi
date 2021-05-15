using AutoMapper;
using Zindagi.Domain.RequestsAggregate;
using Zindagi.Domain.RequestsAggregate.ViewModels;
using Zindagi.Domain.UserAggregate;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

namespace Zindagi.Domain
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<int, Enumeration>().ForMember(dest => dest.Id,
                                                    opt => opt.MapFrom(src => Enumeration.FromValue<Status>(src)));

            CreateMap<string, Enumeration>().ForMember(dest => dest.Id,
                                                       opt => opt.MapFrom(src => Enumeration.FromDisplayName<Status>(src)));

            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<BloodRequest, BloodRequestDto>()
                .ReverseMap();
        }
    }
}
