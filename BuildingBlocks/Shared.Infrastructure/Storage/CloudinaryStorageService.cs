using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Shared.Application.Common.Interfaces;

namespace Shared.Infrastructure.Storage
{
    public class CloudinaryStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryStorageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName, CancellationToken cancellationToken = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folderName,
                // Bạn có thể tận dụng các tính năng thông minh của Cloudinary tại đây
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary Upload Failed: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString(); // Trả về URL ảnh dạng https
        }

        public async Task<bool> DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            // Logic tách PublicId từ URL để xóa ảnh khi cần
            var publicId = ExtractPublicIdFromUrl(fileUrl);
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        private string ExtractPublicIdFromUrl(string fileUrl)
        {
            // Ví dụ URL: https://res.cloudinary.com/demo/image/upload/v12345678/products/my-product-image.jpg
            // => PublicId cần lấy: "products/my-product-image"
            try
            {
                if (string.IsNullOrEmpty(fileUrl)) return string.Empty;

                var uri = new Uri(fileUrl);
                var segments = uri.AbsolutePath.Split('/');

                // Tìm vị trí chữ "upload" trong chuỗi URL
                int uploadIndex = Array.IndexOf(segments, "upload");
                if (uploadIndex == -1) return string.Empty;

                // Các phần tử sau "upload" thường là: ["v12345678", "products", "my-product-image.jpg"]
                // Nếu phần tử tiếp theo bắt đầu bằng chữ 'v' (version), ta bỏ qua 2 phần tử, ngược lại bỏ qua 1
                int skipCount = segments[uploadIndex + 1].StartsWith("v") ? 2 : 1;

                var publicIdSegments = segments.Skip(uploadIndex + skipCount);
                var fullPublicIdWithExtension = string.Join("/", publicIdSegments);

                // Dùng Path để xóa bỏ phần mở rộng (.jpg, .png...) ở cuối file
                return Path.ChangeExtension(fullPublicIdWithExtension, null);
            }
            catch
            {
                // Nếu URL không đúng định dạng của Cloudinary, trả về rỗng để tránh crash app
                return string.Empty;
            }
        }
    }
}
