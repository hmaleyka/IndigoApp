namespace IndigoApp.Helpers
{
    public static class FileManager
    {
        public static string Upload(this IFormFile file, string env, string foldername)
        {
            string filename = file.FileName;

            filename=Guid.NewGuid().ToString();

            string path = env+foldername+filename;

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);

            }
            return filename;
                
                
                
        }
        public static bool CheckLong(this IFormFile file, int length)
        {
            return file.Length <= length;
        }
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
    }
}
