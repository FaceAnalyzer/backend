using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.ApiPlayground.Controllers;

[Route("files")]
[ApiController]
public class FilesController : Controller
{
    private readonly string _filePathBase = "/code/FaceAnalyzer/";
    
   public FilesController(IWebHostEnvironment hostingEnvironment)
    {
        _filePathBase = Path.Combine(hostingEnvironment.ContentRootPath, "..", "uploaded-files/");

    }
    
    [HttpPost("buffer-upload")]
    [Consumes("multipart/form-data")]
 public async Task<IActionResult> UploadVideo([FromForm(Name = "file")]IFormFile file)
    {
        var filePath = Path.Combine( _filePathBase + file.FileName); // Set the desired file path
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok($"File uploaded and saved to disk. to {filePath}");
    }
    [HttpPost("streamed-upload")]
    public async Task<IActionResult> Upload([FromForm(Name = "file")] IFormFile file)
    {
        var bufferSize = (int)file.Length / 5;
        var filePath = Path.Combine( _filePathBase + file.FileName); // Set the desired file path
        await using (var readStream = file.OpenReadStream())
        {
            var buffer = new byte[bufferSize];
            await using (var writeStream = new FileStream(filePath,  FileMode.Append))
            {
                while ((await readStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await writeStream.WriteAsync(buffer);
                }
            }
        }


        return Ok($"File uploaded and saved to disk. to {filePath}");
    }
    [HttpPost("chunked-upload")]
    public async Task<IActionResult> ChunkedUpload([FromForm(Name = "file")] IFormFile file)
    {
        return Ok("To be implemented.");
    }
}