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
        private readonly IMapper _mapper;

        public AddImageHandler(IImageRepository imageRepository, IMapper mapper) 
        { 
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            string? imagePath = null;
            
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(request.Image.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

            using (var stream = File.Create(filePath))
            {
                await request.Image.CopyToAsync(stream);
            }

            imagePath = Path.Combine("images", fileName);

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