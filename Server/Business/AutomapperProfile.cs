using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        //TODO add mappings from Entities to Models
        CreateMap<User, UserModel>()
            .ForMember(um => um.FilesIds, u
                => u.MapFrom(src => src.Files.Select(x => x.Id)));
    }
}