using MediatR;
using Microsoft.AspNetCore.Http;
using SprintManager.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace SprintManager.Application.Commands.Images
{
    public class AddImageCommand : IRequest<ImageDto>
    {
        [Required]
        public Guid WorkItemId { get; set; }

        [Required]
        public IFormFile Image {  get; set; }
    }
}