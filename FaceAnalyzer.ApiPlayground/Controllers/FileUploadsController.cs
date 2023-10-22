using FaceAnalyzer.ApiPlayground.data;
using FaceAnalyzer.ApiPlayground.data.Entities;
using FaceAnalyzer.ApiPlayground.Dto;
using FaceAnalyzer.ApiPlayground.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.ApiPlayground.Controllers;

[Route("[controller]")]
public class FileUploadsController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;

    public FileUploadsController(FileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost("")]
    public Task<FileUpload> AddNewFileUpload([FromBody] FileUpload fileUpload)
    {
        var result = _fileUploadService.Add(fileUpload);
        return result;
    }

    [HttpPost("chunks")]
    public async Task<FileChunk> AddChunk([FromBody] FileChunkDto dto)
    {
        byte[] content = Convert.FromBase64String(dto.Content);

        var chunk = new FileChunk();
        chunk.UploadStatus = UploadStatus.NotStarted;
        chunk.Order = dto.Order;
        chunk.Content = content;
        chunk.FileUploadId = dto.FileId;
        var result = await _fileUploadService.AddChunk(chunk);
        return result;
    }

    [HttpPatch("finish/{id:int}")]
    public IActionResult FinishUpload(int id)
    {
        BackgroundJob.Enqueue<FileWriteTask>(t => t.Execute(id));
        return Ok($"File upload for file with id {id} has started");
    }

    [HttpGet("")]
    public Task<IList<FileUpload>?> Get()
    {
        return _fileUploadService.Get();
    }
}