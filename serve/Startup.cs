using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace serve
{
    public class Startup
    {
#pragma warning disable CA1822 // Mark members as static
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var fileServerOptions = new FileServerOptions {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileProvider = env.WebRootFileProvider
            };

            fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;

            app.UseFileServer(fileServerOptions);
        }
    }
}
