using Microsoft.AspNetCore.Http;
using Shared.Application.Common.Models;

namespace Shared.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName, CancellationToken cancellationToken = default);

        Task<bool> DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
