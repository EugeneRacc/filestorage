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
            CreateMap<User, UserModel>()
                .ForMember(r => r.RoleName, u
                    => u.MapFrom(src => src.Role.RoleName))
                .ForMember(us => us.UsedDiskSpace, u
                    => u.MapFrom(src => src.UsedDiskSpade))
                .ForMember(um => um.FilesIds, u
                    => u.MapFrom(src => src.Files.Select(x => x.Id)))
                .ReverseMap();
            CreateMap<Data.Entities.File, FileModel>()
                .ForMember(um => um.ChildFileIds, u
                    => u.MapFrom(src => src.ChildFiles.Select(x => x.Id)))
                .ReverseMap();
            /*CreateMap<UserModel, User>()
                .ForMember(ur => ur.RoleId, u =>
                u.MapFrom(src => src.Role == "Admin" ? 1 : 2))
                .ForMember(uds => uds.UsedDiskSpade, u =>
                u.MapFrom(src => src.UsedDiskSpace));
                //.ForMember(uf => uf.Files.Where(z => z.UserId == z.User.Id).Select(x => x.Id)), u =>
                //u.MapFrom(src => src.FilesIds));*/
        }
    }
}