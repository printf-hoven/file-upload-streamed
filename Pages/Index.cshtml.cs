
using Journal.Utilities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Journal.Pages;

public class IndexModel : PageModel
{
  // Get the default form options so that we can use them to set the default 
  // limits for request body data.
  private static readonly FormOptions _defaultFormOptions = new();

  public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
  {
    if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType ?? string.Empty))
    {
      return BadRequest("Error - IsMultipartContentType failed.");
    }

    string? boundary;

    try
    {
      boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
    }
    catch (Exception ex)
    {
      return BadRequest($"Exception - {ex.Message}");
    }

    var reader = new MultipartReader(boundary, HttpContext.Request.Body);

    MultipartSection? section = await reader.ReadNextSectionAsync(cancellationToken);

    string? fileUploadSuccessMessage = null;

    while (null != section)
    {
      if (true == ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
      {
        if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        {
          return BadRequest("Error - HasFileContentDisposition failed.");
        }
        else
        {
          // Don't trust the file name sent by the client. To display
          // the file name, HTML-encode the value.
          /*
           MemoryStream not recommended for file uploads > 50MB. So we will save
          the incoming stream directly to a filestream as done below
           */
          if (WebUtility.HtmlEncode(contentDisposition.FileName.Value) is string uploadedFileName)
          {
            string uploadedFileExtension = Path.GetExtension(uploadedFileName);

            // TODO: make checks on file extension allowed, if...else return BadRequest on failure

            // save file under a new name for security
            string fileName = Path.ChangeExtension(Path.GetRandomFileName(), uploadedFileExtension)!;

            string filePath = Path.Combine(Constants.UPLOADS_ROOT, fileName)!;

            using FileStream fs = new(filePath, FileMode.CreateNew);

            await section.Body.CopyToAsync(fs, cancellationToken);

            // TODO: make checks of file allowed file size, file signature, anti-virus, etc.,
            // if fail, send BadRequest, and delete the file

            fileUploadSuccessMessage = $"Saved {uploadedFileName} to disk!";

          } // WebUtility.HtmlEncode
          else
          {
            return BadRequest("Error - bad file name. WebUtility.HtmlEncode failed.");
          }
        }
      }

      // Drain any remaining section body that hasn't been consumed and
      // read the headers for the next section.
      section = await reader.ReadNextSectionAsync(cancellationToken);
    }

    return new JsonResult(fileUploadSuccessMessage ?? "File not saved. Unknown error.");
  }

}