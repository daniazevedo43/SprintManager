using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Images;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Handlers.Images
{
    public class GetImageByIdHandler : IRequestHandler<GetImageByIdQuery, ImageDTO>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public GetImageByIdHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<ImageDTO> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetByIdAsync(request.Id);

            if (image == null) throw new SprintManagerNotFoundException($"Image with ID {request?.Id} not found.");

            return _mapper.Map<ImageDTO>(image);    
        }
    }
}