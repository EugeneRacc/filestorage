using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    /// <summary>
    /// Configuration for mapping models
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(r => r.RoleName, u
                    => u.MapFrom(src => src.Role.RoleName))
                .ForMember(us => us.UsedDiskSpace, u
                    => u.MapFrom(src => src.UsedDiskSpade))
                .ForMember(um => um.FilesIds, u
                    => u.MapFrom(src => src.Files.Select(x => x.Id)))
                .ReverseMap();

            CreateMap<User, UserForUpdateModel>()
                .ForMember(r => r.RoleName, u
                    => u.MapFrom(src => src.Role.RoleName))
                .ReverseMap();

            CreateMap<Data.Entities.File, FileModel>()
                .ForMember(um => um.ChildFileIds, u
                    => u.MapFrom(src => src.ChildFiles.Where(x => x.ParentId != null).Select(y => y.Id)))
                .ReverseMap();
        }
    }
}