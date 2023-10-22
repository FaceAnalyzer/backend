using FaceAnalyzer.ApiPlayground.data;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.ApiPlayground.Dto;

public record FileChunkDto
{

  
    public int FileId { get; set; }
    public string Content { get; set; }

    public int Order { get; set; }
}