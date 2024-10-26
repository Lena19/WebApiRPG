using AutoMapper;
using WebApiRPG.DTOs;
using WebApiRPG.Models;

namespace WebApiRPG.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDTO>()
            .ForMember(d => d.Weapon, 
            opt => opt.MapFrom(src => src.Weapon))
            .ForMember(d => d.Skills,
            opt => opt.MapFrom(src => src.Skills));
        CreateMap<PostCharacterDTO, Character>();

        CreateMap<AddWeaponDTO, Weapon>();
        CreateMap<Weapon, GetWeaponDTO>();

        CreateMap<AddSkillDTO, Skill>();
        CreateMap<Skill, GetSkillDTO>();
    }
}
