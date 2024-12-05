namespace Test.Helper
{
    public interface IFileService
    {
       
        bool IsPdf(IFormFile file);
       
        void Delete(string path);
    }
}
