// Install - Package Microsoft.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.Sqlite
// Install-Package Microsoft.EntityFrameworkCore.Tools
using Journal;
using Journal.Filters;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
  // if not present, will throw similar exception:
  //   Microsoft.AspNetCore.Server.Kestrel.Core.BadHttpRequestException:
  //   Request body too large. The max request body size is 30000000 bytes.
  options.Limits.MaxRequestBodySize = 1L * 1024 * 1024 * 1024; // 1 GB

  // optional: timeout settings
  options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);

  options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddRazorPages(options =>
{
  options.Conventions
          .AddPageApplicationModelConvention("/Index",
              model =>
              {
                model.Filters.Add(
                    new GenerateAntiforgeryTokenCookieAttribute());
                model.Filters.Add(
                    new DisableFormValueModelBindingAttribute());
              });
});

var app = builder.Build();

app.MapRazorPages();

// also create uploads directory, if doesn't exist
Directory.CreateDirectory(Constants.UPLOADS_ROOT);

app.Run();

namespace Journal
{
  public class Constants
  {
    // OR set a path in settings, etc.,
    public const string UPLOADS_ROOT = "Uploads";
  }
}