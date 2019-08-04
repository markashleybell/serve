using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace serve
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
