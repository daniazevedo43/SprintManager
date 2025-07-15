using AutoMapper;
using SprintManager.Application.DTOs;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Mappers
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile() 
        {
            CreateMap<Project, ProjectDTO>();
        }
    }
}
