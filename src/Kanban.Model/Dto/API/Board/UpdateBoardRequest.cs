using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Board;

public class UpdateBoardRequest
{
    [Required]
    public string Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Name { get; set; }
}
