using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Card;

public class CardDto
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
}