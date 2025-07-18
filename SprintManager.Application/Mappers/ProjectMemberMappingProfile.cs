using AutoMapper;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Mappers
{
    public class ProjectMemberMappingProfile : Profile
    {
        public ProjectMemberMappingProfile()
        {
            CreateMap<ProjectMember, ProjectMemberDTO>();
        }
    }
}