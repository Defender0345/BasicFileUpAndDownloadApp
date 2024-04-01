using System.Net.Mime;

namespace BasicFileUpAndDownloadApp.Models
{
    public class UploadModel
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
