namespace UrlLoader.Consumer.Helpers
{
    public static class GetUrlTypeHelper
    {
        public static async Task<string> GetUrlTypeAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Отправка запроса и получение ответа
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Убедиться, что запрос был успешным
                    response.EnsureSuccessStatusCode();

                    // Получение заголовка Content-Type
                    string contentType = response.Content.Headers.ContentType.MediaType;

                    // Список типов для веб-страниц
                    var webPageTypes = new HashSet<string>
                {
                    "text/html",
                    "application/xhtml+xml"
                };

                    // Проверка, является ли контент веб-страницей
                    if (webPageTypes.Contains(contentType))
                    {
                        return "application/pdf";
                    }
                    else
                    {
                        // Извлечение расширения файла из URL
                        string extension = Path.GetExtension(url).ToLower();
                        return string.IsNullOrEmpty(extension) ? "unknown" : extension;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке URL: {ex.Message}");
                return "unknown";
            }
        }

        public static string GetMimeType(string extension)
        {
            // Словарь для хранения MIME-типов для различных расширений файлов
            var mimeTypes = new Dictionary<string, string>
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".webp", "image/webp" },
            { ".pdf", "application/pdf" },
            { ".txt", "text/plain" },
            { ".html", "text/html" },
            { ".htm", "text/html" },
            { ".xml", "application/xml" },
            { ".json", "application/json" },
            { ".zip", "application/zip" },
            { ".tar", "application/x-tar" },
            { ".rar", "application/vnd.rar" },
            { ".mp4", "video/mp4" },
            { ".mp3", "audio/mpeg" }
            // Добавьте другие типы по мере необходимости
        };

            return mimeTypes.TryGetValue(extension, out string mimeType) ? mimeType : "application/octet-stream";
        }
    }
}
