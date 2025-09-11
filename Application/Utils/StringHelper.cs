namespace krov_nad_glavom_api.Application.Utils
{
    public class StringHelper
    {
        public static string GetPublicIdFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL is empty");

            var uri = new Uri(url);

            var filename = Path.GetFileName(uri.LocalPath);

            var publicId = Path.GetFileNameWithoutExtension(filename);

            return publicId;
        }
    }
}