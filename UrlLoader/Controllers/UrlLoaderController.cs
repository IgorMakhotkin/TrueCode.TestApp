using MassTransit;
using MessageContract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using UrlLoader.Database.Model;
using UrlLoader.Producer.Database.Repositories;


namespace UrlLoader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlLoaderController : ControllerBase
    {
        private readonly IRequestClient<DownloadUrlСontent> _client;
        private readonly IFileEntityRepository _fileEntityRepository;
        public UrlLoaderController(IRequestClient<DownloadUrlСontent> client, IFileEntityRepository fileEntityRepository)
        {
            _client = client;
            _fileEntityRepository = fileEntityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> LoadUrlAsync([Required, FromBody] UrlRequestDto request)
        {
            Response<LoadedUrlContent> response = await _client.GetResponse<LoadedUrlContent>(new {Url = request.Url}, timeout: RequestTimeout.After(s: 100));
            await _fileEntityRepository.CreateAsync(new FileEntity() {
                Id = Guid.NewGuid(),
                ContentType = response.Message.TypeContent,
                FileName = response.Message.FileName,
                Data = response.Message.Content
            });
            return File(response.Message.Content, response.Message.TypeContent, response.Message.FileName);
        }
    }
}
