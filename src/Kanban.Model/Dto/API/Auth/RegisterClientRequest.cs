using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Auth;

public class RegisterClientRequest
{
    [Required]
    public ClientDto Client { get; set; }
}
