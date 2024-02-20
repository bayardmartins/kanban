using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Board;

public class CreateBoardRequest
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
}
