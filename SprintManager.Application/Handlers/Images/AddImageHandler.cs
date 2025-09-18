using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Images
{
    public class AddImageHandler : IRequestHandler<AddImageCommand, ImageDTO>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public AddImageHandler(
            IImageRepository imageRepository,
            IWorkItemRepository workItemRepository,
            UserManager<User> userManager,
            IFileStorageService fileStorageService, 
            IMapper mapper
        ) 
        { 
            _imageRepository = imageRepository;
            _workItemRepository = workItemRepository;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            if(!AllowedExtensions.Contains(Path.GetExtension(request.Image.FileName)))
                throw new SprintManagerFileNotAllowedException($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", AllowedExtensions)}.");

            var workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId);
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (!string.IsNullOrWhiteSpace(request.WorkItemId.ToString()) && workItem == null)
                throw new SprintManagerNotFoundException($"Work item with ID {request.WorkItemId} not found.");

            if (!string.IsNullOrWhiteSpace(request.UserId.ToString()) && user == null)
                throw new SprintManagerNotFoundException($"User with ID {request.UserId} not found.");

            var imagePath = _fileStorageService.GetFilePath("Images", request.Image.FileName);

            var image = new Image(
                request.WorkItemId, 
                request.UserId, 
                request.Image.ContentType,
                request.Image.FileName,
                imagePath);

            await _fileStorageService.SaveFileAsync(request.Image, "Images");
            await _imageRepository.AddAsync(image);

            return _mapper.Map<ImageDTO>(image);
        }
    }
}