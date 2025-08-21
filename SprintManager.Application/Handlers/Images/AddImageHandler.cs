using AutoMapper;
using MediatR;
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
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public AddImageHandler(
            IImageRepository imageRepository,
            IWorkItemRepository workItemRepository,
            IUserRepository userRepository,
            IFileStorageService fileStorageService, 
            IMapper mapper
        ) 
        { 
            _imageRepository = imageRepository;
            _workItemRepository = workItemRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            if(!AllowedExtensions.Contains(Path.GetExtension(request.Image.FileName)))
                throw new SprintManagerFileNotAllowedException($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", AllowedExtensions)}.");

            var workItemId = await _workItemRepository.GetByIdAsync(request.WorkItemId);
            var userId = await _userRepository.GetByIdAsync(request.UserId);

            if (!string.IsNullOrWhiteSpace(request.WorkItemId.ToString()) && workItemId == null)
                throw new SprintManagerNotFoundException($"Work item with ID {request.WorkItemId} not found.");

            if (!string.IsNullOrWhiteSpace(request.UserId.ToString()) && userId == null)
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