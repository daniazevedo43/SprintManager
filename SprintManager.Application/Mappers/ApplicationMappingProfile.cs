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
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<ProjectMember, ProjectMemberBasicDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<Sprint, SprintDto>();
            CreateMap<WorkItem, WorkItemDTO>()
                .ForMember(dest => dest.SprintName, opt => opt.MapFrom(src => src.Sprint != null ? src.Sprint.SprintName : null))
                .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.UserName : null));
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.WorkItemTitle, opt => opt.MapFrom(src => src.WorkItem != null ? src.WorkItem.WorkItemTitle : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<Image, ImageDto>()
                .ForMember(dest => dest.WorkItemTitle, opt => opt.MapFrom(src => src.WorkItem != null ? src.WorkItem.WorkItemTitle : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
        }
    }
}