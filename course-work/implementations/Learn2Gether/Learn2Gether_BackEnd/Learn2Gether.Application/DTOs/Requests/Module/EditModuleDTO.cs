using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Module
{
    /// <summary>
    /// Represents the data required to edit an existing module, including module ID and title.
    /// </summary>
    public class EditModuleDTO
    {
        [Required]
        public string ModuleId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;
    }
}
