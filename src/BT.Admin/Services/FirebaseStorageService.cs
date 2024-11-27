using Google.Cloud.Storage.V1;

namespace BT.Admin.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly IConfiguration Configuration;

        public FirebaseStorageService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            Configuration = configuration;
        }

        public async Task<Uri> UploadFile(string name, IFormFile file)
        {
            var randomGuid = Guid.NewGuid();
            var bucketname = Configuration["Firebase:FirebaseStorageBucketname"];

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var blob = await _storageClient.UploadObjectAsync(bucketname, $"{name}-{randomGuid}", file.ContentType, stream);

            var photoUri = new Uri(blob.MediaLink);

            return photoUri;
        }
    }
}
