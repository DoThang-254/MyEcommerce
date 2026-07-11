namespace Shared.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        // Dùng Stream giúp Interface này "sạch", không bị phụ thuộc vào tầng Web (IFormFile)
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName, CancellationToken cancellationToken = default);

        Task<bool> DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
