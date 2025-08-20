using MediatR;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Images
{
    public class RemoveImageHandler : IRequestHandler<RemoveImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IFileStorageService _fileStorageService;

        public RemoveImageHandler (IImageRepository imageRepository, IFileStorageService fileStorageService)
        {
            _imageRepository = imageRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task Handle(RemoveImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetByIdAsync(request.Id);

            if (image == null) throw new SprintManagerNotFoundException($"Image with ID {request?.Id} not found.");

            _fileStorageService.DeleteFile("Images", image.FileName);

            await _imageRepository.DeleteAsync(image);
        }
    }
}