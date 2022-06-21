using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //TODO add mappings from Entities to Models
            CreateMap<User, UserModel>()
                .ForMember(r => r.RoleId, u
                    => u.MapFrom(src => src.RoleId))
                .ForMember(us => us.UsedDiskSpace, u
                    => u.MapFrom(src => src.UsedDiskSpade))
                .ForMember(um => um.FilesIds, u
                    => u.MapFrom(src => src.Files.Select(x => x.Id)))
                .ReverseMap();
            CreateMap<Data.Entities.File, FileModel>()
                .ReverseMap();
        }
    }
}