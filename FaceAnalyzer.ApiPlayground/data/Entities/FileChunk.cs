namespace FaceAnalyzer.ApiPlayground.data.Entities;

// <remarks>
// We Can add chunk size to improve performance for checking completeness of uploaded file
// We Change order to offset to gain more flexibility and allow out of order writing of chunks
// </remarks>
public record FileChunk
{
    
    public int Id { get; set; }
    public int FileUploadId { get; set; }
    public byte[] Content { get; set; }
    public int Order { get; set; }
    public UploadStatus UploadStatus { get; set; }

    public FileUpload FileUpload { get; set; }
}