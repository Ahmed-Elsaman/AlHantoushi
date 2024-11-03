namespace AlHantoushi.Infrastructure.Extensions
{
    public static class UploadFiles
    {
        public async static Task<string> UploadFile(this IFormFile file, string folderName)
        {
            string webRootPath = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine(webRootPath, "wwwroot", "Files", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string extension = Path.GetExtension(file.FileName);
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileName = originalFileName;

            string filePath = Path.Combine(folderPath, fileName + extension);

            if (File.Exists(filePath))
            {
                string existingRelativePath = Path.Combine("Files", folderName, fileName + extension).Replace("\\", "/");
                return existingRelativePath;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string relativePath = Path.Combine("Files", folderName, fileName + extension).Replace("\\", "/");

            return relativePath;
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
