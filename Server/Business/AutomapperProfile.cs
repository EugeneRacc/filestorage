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
            .ReverseMap();
    }
}