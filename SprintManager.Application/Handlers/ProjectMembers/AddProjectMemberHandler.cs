using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.ProjectMembers;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.ProjectMembers
{
    public class AddProjectMemberHandler : IRequestHandler<AddProjectMemberCommand, ProjectMemberDTO>
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public AddProjectMemberHandler(IProjectMemberRepository projectMemberRepository, IMapper mapper) 
        {
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task<ProjectMemberDTO> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var existingProjectMember = await _projectMemberRepository.GetByUserAndProjectIdAsync(request.UserId, request.ProjectId);

            if (existingProjectMember != null) throw new SprintManagerConflictException($"A user with ID {request.UserId} is already assigned to a project with ID {request.ProjectId}.");

            var projectMember = new ProjectMember(request.ProjectId, request.UserId, request.Role);
            
            await _projectMemberRepository.AddAsync(projectMember);
            
            return _mapper.Map<ProjectMemberDTO>(projectMember);
        }
    }
}