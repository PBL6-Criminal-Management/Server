using System.ComponentModel.DataAnnotations;
using Application.Dtos.Requests.Image;
using Domain.Constants;

namespace Application.Dtos.Requests.Evidence
{
    public class EvidenceRequest
    {
        public long Id { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_DESCRIPTION)]
        public string? Description { get; set; }
        public List<ImageRequest>? EvidenceImages { get; set; }

    }
}