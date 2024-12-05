using System.ComponentModel.DataAnnotations.Schema;
using Test.Models.Common;

namespace Test.Models
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        [NotMapped]
        public IFormFile FormFile { get; set; }
    }
}
