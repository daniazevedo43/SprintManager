using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class AddProjectMemberHandler : IRequestHandler<AddProjectMemberCommand, ProjectMemberDto>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AddProjectMemberHandler(
            IProjectMemberRepository projectMemberRepository,
            IProjectRepository projectRepository,
            UserManager<User> userManager,
            IMapper mapper
        ) 
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ProjectMemberDto> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var existingProjectMember = await _projectMemberRepository.GetByUserAndProjectIdAsync(request.UserId, request.ProjectId);
            
            if (existingProjectMember != null) throw new SprintManagerConflictException($"A user with ID {request.UserId} is already assigned to a project with ID {request.ProjectId}.");

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (project == null) throw new SprintManagerNotFoundException($"Project with ID {request.ProjectId} not found.");
            if (user == null) throw new SprintManagerNotFoundException($"User with ID {request.UserId} not found.");

            var projectMember = new ProjectMember(request.ProjectId, request.UserId, request.Role);
            
            await _projectMemberRepository.AddAsync(projectMember);
            
            return _mapper.Map<ProjectMemberDto>(projectMember);
        }
    }
}