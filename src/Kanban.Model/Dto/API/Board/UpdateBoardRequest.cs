using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Board;

public class UpdateBoardRequest
{
    [Required]
    public string Id { get; set; }

    [Required(ErrorMessage = "This fields needs a value")]
    [MinLength(3,ErrorMessage = "Invalid Length")]
    public string Name { get; set; }
}
