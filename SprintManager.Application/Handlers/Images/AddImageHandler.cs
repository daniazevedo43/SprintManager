using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;
using System.Security.Claims;

namespace SprintManager.Application.Handlers.Images
{
    public class AddImageHandler : IRequestHandler<AddImageCommand, ImageDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageRepository _imageRepository;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public AddImageHandler(
            IHttpContextAccessor httpContextAccessor,
            IImageRepository imageRepository,
            IWorkItemRepository workItemRepository,
            UserManager<User> userManager,
            IFileStorageService fileStorageService, 
            IMapper mapper
        ) 
        { 
            _httpContextAccessor = httpContextAccessor;
            _imageRepository = imageRepository;
            _workItemRepository = workItemRepository;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ImageDto> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated.");

            if (!AllowedExtensions.Contains(Path.GetExtension(request.Image.FileName)))
                throw new SprintManagerFileNotAllowedException($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", AllowedExtensions)}.");

            var workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (!string.IsNullOrWhiteSpace(request.WorkItemId.ToString()) && workItem == null)
                throw new SprintManagerNotFoundException($"Work item with ID {request.WorkItemId} not found.");

            if (!string.IsNullOrWhiteSpace(userId.ToString()) && user == null)
                throw new SprintManagerNotFoundException($"User with ID {userId} not found.");

            var imagePath = _fileStorageService.GetFilePath("Images", request.Image.FileName);

            var image = new Image(
                request.WorkItemId,
                userId, 
                request.Image.ContentType,
                request.Image.FileName,
                imagePath);

            await _fileStorageService.SaveFileAsync(request.Image, "Images");
            await _imageRepository.AddAsync(image);

            return _mapper.Map<ImageDto>(image);
        }
    }
}