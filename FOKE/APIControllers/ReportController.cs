using FOKE.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FOKE.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("Download")]
        public async Task<ActionResult> Download(string tFile, string fileName)
        {
            var mfile = await GenericUtilities.GetReportData(tFile);
            return File(mfile, GetContentType(fileName), Path.GetFileName(fileName));
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
