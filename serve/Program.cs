using System;
using System.IO;
using System.Runtime.Versioning;
using DocoptNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Win32;

namespace serve
{
    public static class Program
    {
        private const int DefaultHttpsPort = 5001;

        private const string USAGE = @"
Usage: serve [<path>] [--port=PORT]
       serve (--register|--unregister)

-h --help     Show this
--port=PORT   Specify the HTTPS port
--register    Set up context menu items (WINDOWS ONLY)
--unregister  Remove context menu items (WINDOWS ONLY)
";

        private static readonly string[] _contextMenuRegistryClasses = new[] {
            "Directory",
            "Directory\\Background"
        };

        public static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(USAGE, args, version: "serve 0.0.1", exit: true);

            var register = arguments["--register"]?.IsTrue == true;
            var unregister = arguments["--unregister"]?.IsTrue == true;

            if (register)
            {
                if (OperatingSystem.IsWindows())
                {
                    RegisterContextMenu();
                    Console.WriteLine("Context menu items added.");
                }
                else
                {
                    Console.WriteLine("--register is not supported on this operating system.");
                }

                Environment.Exit(0);
            }

            if (unregister)
            {
                if (OperatingSystem.IsWindows())
                {
                    UnregisterContextMenu();
                    Console.WriteLine("Context menu items removed.");
                }
                else
                {
                    Console.WriteLine("--unregister is not supported on this operating system.");
                }

                Environment.Exit(0);
            }

            var path = arguments["<path>"] != null
                ? arguments["<path>"].ToString()
                : Directory.GetCurrentDirectory();

            Environment.CurrentDirectory = path;

            var httpsPort = arguments["--port"] != null
                ? arguments["--port"].AsInt
                : DefaultHttpsPort;

            var httpPort = httpsPort - 1;

            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .UseContentRoot(path)
                .UseWebRoot(path)
                .UseUrls($"http://localhost:{httpPort}", $"https://localhost:{httpsPort}")
                .Build()
                .Run();
        }

        [SupportedOSPlatform("windows")]
        private static void RegisterContextMenu()
        {
            if (OperatingSystem.IsWindows())
            {
                var location = Path.Join(AppContext.BaseDirectory, "serve.exe");

                foreach (var registryClass in _contextMenuRegistryClasses)
                {
                    AddContextMenuRegistryKey(location, registryClass);
                }
            }
        }

        [SupportedOSPlatform("windows")]
        private static void UnregisterContextMenu()
        {
            foreach (var registryClass in _contextMenuRegistryClasses)
            {
                RemoveContextMenuRegistryKey(registryClass);
            }
        }

        [SupportedOSPlatform("windows")]
        private static void RemoveContextMenuRegistryKey(string registryClass) =>
            Registry.CurrentUser.DeleteSubKeyTree($@"SOFTWARE\Classes\{registryClass}\shell\servedotexe");

        [SupportedOSPlatform("windows")]
        private static void AddContextMenuRegistryKey(string location, string registryClass)
        {
            using var key = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\Classes\{registryClass}\shell\servedotexe");

            key.SetValue(string.Empty, $"Serve Files at https://localhost:{DefaultHttpsPort}", RegistryValueKind.String);
            key.SetValue("Icon", location, RegistryValueKind.String);

            var command = location.EndsWith(".dll")
                ? $"\"C:\\Program Files\\dotnet\\dotnet.exe\" \"{location}\" \"%V\""
                : $"\"{location}\" \"%V\"";

            using var commandKey = key.CreateSubKey("command");

            commandKey.SetValue(string.Empty, command, RegistryValueKind.String);
        }
    }
}
