using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Card;

public class CreateCardRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}
