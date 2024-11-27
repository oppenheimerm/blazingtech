using BT.Admin.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;

namespace BT.Admin.Services.Files
{
    public class PhotoService : IPhotoService
    {
        readonly ILogger<PhotoService> _logger;
        readonly IWebHostEnvironment _environment;

        public PhotoService(ILogger<PhotoService> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async Task<(FileInfo? fileInfo, bool Success, string ErrorMessage)> AddPhotoAsync(IBrowserFile file, string newFilename)
        {
            var validFile = FileHelpers.ValidImageFile(file);
            if (validFile)
            {
                try {

                    string filePath = Path.Combine(_environment.WebRootPath, FileHelpers.RootDirectory, newFilename);

                    if (File.Exists(filePath))
                    {
                        filePath = Path.ChangeExtension(filePath, file.GetHashCode() + Path.GetExtension(filePath));
                    }


                    const int MAX_FILESIZE = 5000 * 1024; // 2 MB
                    var fileStream = file.OpenReadStream(MAX_FILESIZE);
                    //var extension = Path.GetExtension(newFilename);
                    ///var targetFilePath = Path.ChangeExtension(randomFile, extension);
                    var destinationStream = new FileStream(filePath, FileMode.Create);
                    await fileStream.CopyToAsync(destinationStream);
                    destinationStream.Close();

                    var fiileInfo = new FileInfo(filePath);
                    destinationStream = null;
                    _logger.LogInformation($"Photo: {file.Name} saved to path: {filePath} at {DateTime.UtcNow}");
                    return (new FileInfo(filePath), true, string.Empty);

                }
                catch(Exception err)
                {
                    _logger.LogError($"Failed to add {newFilename} with the following error { err.ToString()} at {DateTime.UtcNow}");
                    return (new FileInfo(string.Empty), false, "Not a valid file");
                }
            }
            else
            {
                _logger.LogError($"The file: {newFilename} is not a valid file name");
                return (new FileInfo(string.Empty), false, "Not a valid file");
            }

        }

    }

}
