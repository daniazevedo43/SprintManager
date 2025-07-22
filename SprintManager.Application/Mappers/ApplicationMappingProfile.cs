using AutoMapper;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Mappers
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile() 
        {
            CreateMap<User, UserDTO>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectMember, ProjectMemberDTO>();
            CreateMap<ProjectMember, ProjectMemberBasicDTO>();
            CreateMap<Sprint, SprintDTO>();
        }
    }
}