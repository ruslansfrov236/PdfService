using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.ViewModels.Person
{
    public class CreatePersonDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
       
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
        [Required]
        [NotMapped]
        public IFormFile FormFile { get; set; }
    }
}
