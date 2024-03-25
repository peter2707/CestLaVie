using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Value == null)
            {
                throw new ArgumentException("CloudinarySettings must not be null", nameof(config));
            }

            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            try
            {
                _cloudinary = new Cloudinary(acc);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Failed to initialize Cloudinary", ex);
            }
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty", nameof(file));
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "cestlavie_bucket"
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                throw new ArgumentException("PublicId is null or empty", nameof(publicId));
            }

            var deleteParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            return deletionResult;
        }
    }
}
