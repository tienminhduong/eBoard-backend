using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using eBoardAPI.Interfaces.Services;

namespace eBoardAPI.Services
{
    public class PhotoService: IPhotoService
    {
        const string CLOUD_NAME = "CLOUDINARY_CLOUD_NAME";
        const string API_KEY = "CLOUDINARY_API_KEY";
        const string API_SECRET = "CLOUDINARY_API_SECRET";
        private readonly Cloudinary _cloudinary;
        public PhotoService()
        {
            var cloudName = Environment.GetEnvironmentVariable(CLOUD_NAME);
            var apiKey = Environment.GetEnvironmentVariable(API_KEY);
            var apiSecret = Environment.GetEnvironmentVariable(API_SECRET);
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500)
                                                         .Width(500)
                                                         .Crop("fill")
                                                         .Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}
