namespace YourRest.Application.Dto;

public class FileDto
{
    public Stream Stream { get; set; }

    public string MimeType { get; set; }

    public string FileName { get; set; }
}
