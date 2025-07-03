using AutoMapper;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserDTO>();
        }
    }
}
