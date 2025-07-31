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
            CreateMap<WorkItem, WorkItemDTO>()
                // Maps Sprint.SprintName to SprintName in DTO, with null verification
                .ForMember(dest => dest.SprintName, opt => opt.MapFrom(src => src.Sprint != null ? src.Sprint.SprintName : null));
            CreateMap<Comment, CommentDTO>()
                // Maps WorkItem.WorkItemTitle to WorkItemTitle in DTO, with null verification
                .ForMember(dest => dest.WorkItemTitle, opt => opt.MapFrom(src => src.WorkItem != null ? src.WorkItem.WorkItemTitle : null));
        }
    }
}