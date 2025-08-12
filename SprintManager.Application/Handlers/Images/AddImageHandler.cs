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
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public AddImageHandler(IImageRepository imageRepository, IFileStorageService fileStorageService, IMapper mapper) 
        { 
            _imageRepository = imageRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            if(!AllowedExtensions.Contains(Path.GetExtension(request.Image.FileName)))
                throw new SprintManagerFileNotAllowedException($"File extension not allowed. Please upload a file with the following extensions: {string.Join(", ", AllowedExtensions)}.");

            var imagePath = await _fileStorageService.SaveFileAsync(request.Image, "Images");

            var image = new Image(
                request.WorkItemId, 
                request.UserId, 
                request.Image.ContentType,
                request.Image.FileName,
                imagePath);

            await _imageRepository.AddAsync(image);

            return _mapper.Map<ImageDTO>(image);
        }
    }
}