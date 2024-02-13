using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Card;

public class UpdateCardRequest
{
    [Required]
    public CardDto Card { get; set; }
}
