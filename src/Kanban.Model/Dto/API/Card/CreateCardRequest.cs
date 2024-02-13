using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Card;

public class CreateCardRequest
{
    [Required]
    public CardDto Card { get; set; }
}
