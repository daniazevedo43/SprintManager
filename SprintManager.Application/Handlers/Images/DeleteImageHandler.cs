using AutoMapper;
using MediatR;
using SprintManager.Application.Commands.Images;
using SprintManager.Application.Interfaces;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Images
{
    public class DeleteImageHandler : IRequestHandler<DeleteImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public DeleteImageHandler (IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }


        public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetByIdAsync(request.Id);

            if (image == null) throw new SprintManagerNotFoundException($"Image with ID {request?.Id} not found.");
        
            await _imageRepository.DeleteAsync(image);
        }
    }
}