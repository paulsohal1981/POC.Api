using AutoMapper;
using POC.Api.Model;

namespace POC.Api.Mappings.Profiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<City, County>()
                 .ForMember(d => d.CountyName, o => o.MapFrom(s => s.Name));
        }
    }
}
