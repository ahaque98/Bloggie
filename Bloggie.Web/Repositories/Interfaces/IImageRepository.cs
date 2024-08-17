namespace Bloggie.Web.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
