using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Auth;

public class ClientDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Secret { get; set; } = string.Empty;
}
