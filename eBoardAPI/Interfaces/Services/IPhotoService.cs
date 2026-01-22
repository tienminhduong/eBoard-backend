using CloudinaryDotNet.Actions;

namespace eBoardAPI.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
