# serve.exe

Command-line utility to serve static files from a folder, using the Kestrel web server.

## Prerequisites

You'll need the [.NET Core 2.2 Runtime](https://dotnet.microsoft.com/download/thank-you/dotnet-runtime-2.2.6-windows-hosting-bundle-installer) installed.

## Why?

Sometimes it's handy to quickly spin up a web server which points at a folder, particularly when playing with client-side code or a static blog generator. 

I've always previously used `python -m http.server` from within a folder to accomplish this, but a) there's a dependency on Python, and b) this won't serve files over HTTPS without some tweaking ([info here](https://blog.anvileight.com/posts/simple-python-http-server/)).

`serve` directly supports HTTPS, using the .NET Core local development certificate which gets installed<sup name="a1">[*](#f1)</sup> when creating a new web project using the .NET Core SDK.

## Usage

`serve [<path>] [--port=PORT]`

You can either run `serve` from within the directory you wish to start serving, or from elsewhere by specifying the `path` argument. 

The default HTTPS port is `5000`, but you can override this if you wish; the HTTP port will just be the HTTPS port number + 1 (so default `5001`).

## Thanks

This utility is based on code from [this article](https://www.meziantou.net/starting-a-http-file-server-from-the-file-explorer-using-dotnet-core-2-0-and-kestrel.htm) by Gérald Barré.

<br />

<small name="f1"><b>*</b> If it isn't, you can run `dotnet dev-certs https --trust` to install it manually. [↩](#a1)</small>