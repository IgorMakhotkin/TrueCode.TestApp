
using PuppeteerSharp;
using PuppeteerSharp.BrowserData;
using PuppeteerSharp.Media;

namespace UrlLoader.Consumer
{
    public static class DownloadUrlHelper
    {
        public static async Task<byte[]> GetPdfFromUrlAsync(string url)
        {
            await new BrowserFetcher().DownloadAsync(Chrome.DefaultBuildId);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(url, new NavigationOptions
            {
                Timeout = 100000 // Устанавливаем тайм-аут в 100 секунд
            });
            byte[] pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Bottom = "20px",
                    Left = "20px",
                    Right = "20px"
                }
            });
            return pdfBytes;
        }

        public static async Task<byte[]> GetFileFromUrlAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Отправка запроса и получение ответа
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Убедиться, что запрос был успешным
                    response.EnsureSuccessStatusCode();

                    // Чтение содержимого ответа как массива байтов
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании файла: {ex.Message}");
                return null;
            }
        }
    }
}
