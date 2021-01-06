# serve.exe

Command-line utility to serve static files from a folder, using the Kestrel web server.

## Prerequisites

You'll need the [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) to build this project.

## Why?

Sometimes it's handy to quickly spin up a web server which points at a folder, particularly when playing with client-side code or a static blog generator. 

I've always previously used `python -m http.server` from within a folder to accomplish this, but a) there's a dependency on Python, and b) this won't serve files over HTTPS without some tweaking ([info here](https://blog.anvileight.com/posts/simple-python-http-server/)).

`serve` directly supports HTTPS, using the .NET Core local development certificate which gets installed<sup>*</sup> when creating a new web project using the .NET Core SDK.

## Usage

`serve [<path>] [--port=PORT]`

You can either run `serve` from within the directory you wish to start serving, or from elsewhere by specifying the `path` argument. The default HTTPS port is `5000`, but you can override this if you wish; the HTTP port will just be the HTTPS port number + 1 (so the default is `5001`).

## Context menu (Windows only)

`serve --register` installs context menu items, which will allow you to right-click a folder in Windows Explorer and start serving it. To remove the menu items, run `serve --unregister`.

## Thanks

This utility is based on code from [this article](https://www.meziantou.net/starting-a-http-file-server-from-the-file-explorer-using-dotnet-core-2-0-and-kestrel.htm) by Gérald Barré.

<br />

**\*** If it isn't, you can run `dotnet dev-certs https --trust` to install it manually.
