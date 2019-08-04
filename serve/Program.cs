using DocoptNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace serve
{
    public class Program
    {
        internal const string USAGE = @"Usage: serve [<path>] [--port=PORT]

-h --help   show this
--port=PORT   https port
";

        public static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(USAGE, args, version: "serve 0.0.1", exit: true);

            var path = arguments["<path>"] != null
                ? arguments["<path>"].ToString()
                : Directory.GetCurrentDirectory();

            Environment.CurrentDirectory = path;

            var httpsPort = arguments["--port"] != null
                ? arguments["--port"].AsInt
                : 5000;

            var httpPort = httpsPort + 1;

            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .UseContentRoot(path)
                .UseWebRoot(path)
                .UseUrls($"http://localhost:{httpPort}", $"https://localhost:{httpsPort}")
                .Build()
                .Run();
        }
    }
}
