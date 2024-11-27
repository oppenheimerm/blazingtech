namespace BT.Admin.Services
{
    public interface IFirebaseStorageService
    {
        public Task<Uri> UploadFile(string name, IFormFile file);
    }
}
