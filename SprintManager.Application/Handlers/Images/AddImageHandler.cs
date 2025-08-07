using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Domain.Entities;

namespace SprintManager.Application.Handlers.Images
{
    public class AddImageHandler : IRequestHandler<AddImageCommand, ImageDTO>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public AddImageHandler(IImageRepository imageRepository, IFileStorageService fileStorageService, IMapper mapper) 
        { 
            _imageRepository = imageRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
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