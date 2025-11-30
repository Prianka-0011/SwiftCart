using Microsoft.AspNetCore.Http; // Requires the FrameworkReference above
using System.Threading.Tasks;

namespace SwiftCart.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string fileName, string folderName);
    }
}