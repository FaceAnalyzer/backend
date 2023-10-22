namespace FaceAnalyzer.ApiPlayground.data.Entities;

public record FileUpload
{
    public int Id { get; set; }
    public string Name  { get; set; }
    public ICollection<FileChunk> Chunks { get; set; }
    public long Size { get; set; }
    public UploadStatus UploadStatus { get; set; } 
}
