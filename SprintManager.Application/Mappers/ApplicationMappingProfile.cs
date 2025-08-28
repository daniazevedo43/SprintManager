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
            CreateMap<ProjectMember, ProjectMemberDTO>()
                // Maps User.UserName to ProjectMember.UserName in DTO, with null verification
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<ProjectMember, ProjectMemberBasicDTO>()
                // Maps User.UserName to ProjectMemberBasicDTO.UserName in DTO, with null verification
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<Sprint, SprintDTO>();
            CreateMap<WorkItem, WorkItemDTO>()
                // Maps Sprint.SprintName to WorkItem.SprintName in DTO, with null verification
                .ForMember(dest => dest.SprintName, opt => opt.MapFrom(src => src.Sprint != null ? src.Sprint.SprintName : null))
                // Maps User.SprintName to WorkItem.SprintName in DTO, with null verification
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<Comment, CommentDTO>()
                // Maps WorkItem.WorkItemTitle to Comment.WorkItemTitle in DTO, with null verification
                .ForMember(dest => dest.WorkItemTitle, opt => opt.MapFrom(src => src.WorkItem != null ? src.WorkItem.WorkItemTitle : null));
            CreateMap<Image, ImageDTO>()
                // Maps WorkItem.WorkItemTitle to Image.WorkItemTitle in DTO, with null verification
                .ForMember(dest => dest.WorkItemTitle, opt => opt.MapFrom(src => src.WorkItem != null ? src.WorkItem.WorkItemTitle : null));
        }
    }
}