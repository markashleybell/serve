using DocoptNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace serve
{
    public class Program
    {
        private const int _defaultHttpsPort = 5000;

        private const string USAGE = @"
Usage: serve [<path>] [--port=PORT]
       serve (--register|--unregister)

-h --help     Show this
--port=PORT   Specify the HTTPS port
--register    Set up context menu items
--unregister  Remove context menu items
";


        private static readonly string[] _contextMenuRegistryClasses = new[] {
            @"Directory",
            @"Directory\Background"
        };

        public static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(USAGE, args, version: "serve 0.0.1", exit: true);

            var register = arguments["--register"]?.IsTrue == true;
            var unregister = arguments["--unregister"]?.IsTrue == true;

            if (register)
            {
                RegisterContextMenu();
                Console.WriteLine("Context menu items added.");
                Environment.Exit(0);
            }

            if (unregister)
            {
                UnregisterContextMenu();
                Console.WriteLine("Context menu items removed.");
                Environment.Exit(0);
            }

            var path = arguments["<path>"] != null
                ? arguments["<path>"].ToString()
                : Directory.GetCurrentDirectory();

            Environment.CurrentDirectory = path;

            var httpsPort = arguments["--port"] != null
                ? arguments["--port"].AsInt
                : _defaultHttpsPort;

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

        private static void RegisterContextMenu()
        {
            var location = Assembly.GetEntryAssembly().Location;

            foreach (var registryClass in _contextMenuRegistryClasses)
            {
                AddContextMenuRegistryKey(location, registryClass);
            }
        }

        private static void UnregisterContextMenu()
        {
            foreach (var registryClass in _contextMenuRegistryClasses)
            {
                RemoveContextMenuRegistryKey(registryClass);
            }
        }

        private static void RemoveContextMenuRegistryKey(string registryClass) =>
            Registry.CurrentUser.DeleteSubKeyTree($@"SOFTWARE\Classes\{registryClass}\shell\servedotexe");

        private static void AddContextMenuRegistryKey(string location, string registryClass)
        {
            using (var key = Registry.CurrentUser.CreateSubKey($@"SOFTWARE\Classes\{registryClass}\shell\servedotexe"))
            {
                key.SetValue(string.Empty, $"Serve Files at https://localhost:{_defaultHttpsPort}", RegistryValueKind.String);
                key.SetValue("Icon", location, RegistryValueKind.String);

                using (var commandKey = key.CreateSubKey("command"))
                {
                    var command = location.EndsWith(".dll")
                        ? $"\"C:\\Program Files\\dotnet\\dotnet.exe\" \"{location}\" \"%V\""
                        : $"\"{location}\" \"%V\"";

                    commandKey.SetValue(string.Empty, command, RegistryValueKind.String);
                }
            }
        }
    }
}
