using AutoMapper;
using MediatR;
using SprintManager.Application.DTOs;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Images;

namespace SprintManager.Application.Handlers.Images
{
    public class GetAllImagesHandler : IRequestHandler<GetAllImagesQuery, List<ImageDTO>>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        
        public GetAllImagesHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<List<ImageDTO>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            var images = await _imageRepository.GetAllAsync();

            return _mapper.Map<List<ImageDTO>>(images);
        }
    }
}