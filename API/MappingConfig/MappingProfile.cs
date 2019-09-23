using AutoMapper;
using API.DTOs;
using API.Models;

namespace API.MappingConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MetaDataDto, MetaData>().ReverseMap();
            CreateMap<TeamDto, MetaData>().ReverseMap();
            CreateMap<ImpactStatsDto, MetaData>().ReverseMap();
        }
    }
}