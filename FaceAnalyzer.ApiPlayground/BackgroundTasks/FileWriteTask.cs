using FaceAnalyzer.ApiPlayground.data;
using FaceAnalyzer.ApiPlayground.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.ApiPlayground;

public class FileWriteTask
{
    private readonly AppDbContext _dbContext;
    private readonly string _filePathBase = "/code/FaceAnalyzer/";

    public FileWriteTask(AppDbContext dbContext, IWebHostEnvironment hostingEnvironment)
    {
        _dbContext = dbContext;
        _filePathBase = Path.Combine(hostingEnvironment.ContentRootPath, "..", "uploaded-files/");
    }

    public async Task Execute(int fileUploadId)
    {
        var fileUpload = await _dbContext.FileUploads
            .FirstOrDefaultAsync(f => f.Id == fileUploadId);
        if (fileUpload is null)
        {
            throw new ArgumentException($"file with id {fileUploadId} does not exists");
        }

        if (fileUpload.UploadStatus == UploadStatus.Finished)
        {
            throw new ArgumentException("This file is already finished");
        }

        if (fileUpload.UploadStatus == UploadStatus.Processing)
        {
            throw new ArgumentException("This file is started by another process");
        }

        // AssertChunks(fileUpload); // [!] This check is very consuming 
        // Change status to Processing avoiding other tasks to be performed on this file.
        fileUpload.UploadStatus = UploadStatus.Processing;
        await _dbContext.SaveChangesAsync();
        var filePath = Path.Combine(_filePathBase, fileUpload.Name);
        var chunks =
            _dbContext.FileChunks.Where(c => c.FileUploadId == fileUploadId)
                .OrderBy(
                    c => c.Order
                );
        foreach (var chunk in chunks)
        {
            await Append(filePath, chunk);
        }

        fileUpload.UploadStatus = UploadStatus.Finished;
        await _dbContext.SaveChangesAsync();
    }


    private async Task Append(string filePath, FileChunk chunk)
    {
        await using var writeStream = new FileStream(filePath, FileMode.Append);
        await writeStream.WriteAsync(chunk.Content);
        chunk.UploadStatus = UploadStatus.Finished;
    }

    private void AssertChunks(FileUpload fileUpload)
    {
        var chunks = fileUpload.Chunks;
        var chunkSize = chunks.Sum(c => c.Content.Length);
        if (chunkSize != fileUpload.Size)
        {
            throw new ArgumentException("Chunk size does not correspond to the actual file size");
        }
    }
}