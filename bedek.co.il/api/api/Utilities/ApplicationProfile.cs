using api.Models;
using Api.Dtos;
 

namespace Api.Utilities
{
    public class ApplicationProfile : AutoMapper.Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
        }
    }
}
