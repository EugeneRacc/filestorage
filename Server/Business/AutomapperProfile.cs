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
            .ForMember(r => r.Role, u 
                => u.MapFrom(src => src.Role.Name))
            .ForMember(us => us.UsedDiskSpace, u 
                => u.MapFrom(src => double.Parse(src.UsedDiskSpade)))
            .ForMember(um => um.FilesIds, u
                => u.MapFrom(src => src.Files.Select(x => x.Id)))
            .ReverseMap();
        CreateMap<Data.Entities.File, FileModel>()
            .ReverseMap();
    }
}