using BasicFileUpAndDownloadApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Reflection;

namespace BasicFileUpAndDownloadApp.Controllers
{
    public class UploadController : Controller
    {
        // /Upload
        // Gets all current files in the 'uploads' folder and displays them on the page
        public IActionResult Index()
        {
            return View(Directory.GetFiles("uploads").Select(Path.GetFileName));
        }

        // /Upload/Upload
        // Show the upload page where you can select a file for uploading
        public IActionResult Upload()
        {
            return View();
        }

        // /Upload/Upload
        // Upload the file to a local folder 'uploads'
        [HttpPost]
        public async Task<IActionResult> Upload(UploadModel model)
        {
            if (model.File != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", model.File.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                return RedirectToAction("Index");
            }

            return BadRequest("File was not uploaded");
        }

        // /Upload
        // Download the file from the 'uploads' folder.
        [HttpGet]
        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            else
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, GetContentType(fileName), fileName);
            }
        }

        //  /Upload
        // Delete the file from the 'uploads' folder
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            if(!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            else
            {
                System.IO.File.Delete(filePath);
                return RedirectToAction(nameof(Index));
            }
        }

        //  Get the Content-Type for the download method
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if(!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            
            return contentType;
        }
    }
}
