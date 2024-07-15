using MassTransit;
using UrlLoader.Consumer.Helpers;

namespace UrlLoader.Consumer
{
    public class LoaderConsumer : IConsumer<DownloadUrlСontent>
    {
        public Task Consume(ConsumeContext<DownloadUrlСontent> context)
        {
            var contetnType = GetUrlTypeHelper.GetUrlTypeAsync(context.Message.Url).Result;
            switch (contetnType)
            {
                case "application/pdf":
                    return context.RespondAsync(new LoadedUrlContent
                    {
                        FileName = context.Message.Url + ".pdf",
                        Content = DownloadUrlHelper.GetPdfFromUrlAsync(context.Message.Url).Result,
                        TypeContent = "application/pdf"
                    });
                default:
                    return context.RespondAsync(new LoadedUrlContent
                    {
                        FileName = context.Message.Url + contetnType,
                        Content = DownloadUrlHelper.GetFileFromUrlAsync(context.Message.Url).Result,
                        TypeContent = GetUrlTypeHelper.GetMimeType(contetnType)
                    });
            }
        }
    }
}
