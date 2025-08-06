using MediatR;
using Microsoft.AspNetCore.Http;
using SprintManager.Application.DTOs;

namespace SprintManager.Application.Commands.Images
{
    public class AddImageCommand : IRequest<ImageDTO>
    {
        public Guid WorkItemId { get; set; }
        public Guid UserId { get; set; }
        public IFormFile Image {  get; set; }
    }
}