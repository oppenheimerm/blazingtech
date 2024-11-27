using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace BT.Admin.Helpers
{
    public interface IPhotoService
    {
        Task<(FileInfo? fileInfo, bool Success, string ErrorMessage)> AddPhotoAsync(IBrowserFile file, string newFilename);
        //Task<(DirectoryInfo directoryInfo, bool Success, string ErrorMessage)> CreateFolderAsync(string name);
    }
}
