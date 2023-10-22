using FaceAnalyzer.ApiPlayground.data;
using FaceAnalyzer.ApiPlayground.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.ApiPlayground.Services;

public class FileUploadService
{
    private readonly AppDbContext _dbContext;

    public FileUploadService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FileUpload> Add(FileUpload fileUpload)
    {
        fileUpload.UploadStatus = UploadStatus.NotStarted;
        _dbContext.FileUploads.Add(fileUpload);
        await _dbContext.SaveChangesAsync();
        return fileUpload;
    }

    public async Task<FileChunk> AddChunk(FileChunk chunk)
    {
        chunk.UploadStatus = UploadStatus.NotStarted;
        _dbContext.FileChunks.Add(chunk);
        await _dbContext.SaveChangesAsync();
        return chunk;
    }

    public async Task Finish(int fileUploadId)
    {
        var fileUpload = await _dbContext.FileUploads
            .FirstOrDefaultAsync(f => f.Id == fileUploadId);
        if (fileUpload is null)
        {
            throw new InvalidDataException("File upload not found");
        }

        fileUpload.UploadStatus = UploadStatus.Finished;
        await _dbContext.SaveChangesAsync();
    }

    public Task<FileUpload?> Get(int id)
    {
        return _dbContext
            .FileUploads
            .FirstOrDefaultAsync(f => f.Id == id);
    }
    public async Task<IList<FileUpload>> Get()
    {
        var result =await _dbContext
            .FileUploads
            .ToListAsync();
        return result;
    }
}