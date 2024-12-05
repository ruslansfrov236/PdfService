using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Test.ViewModels.Person
{
    public class UpdatePersonDto
    {
        public int Id { get; set;  }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
