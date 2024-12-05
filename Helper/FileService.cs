namespace Test.Helper
{
    public class FileService:IFileService
    {
        

        public bool IsPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return false;


            var allowedExtensions = new[] { ".pdf" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }

       
        public void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
