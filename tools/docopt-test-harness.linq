<Query Kind="Program">
  <NuGetReference>docopt.net</NuGetReference>
  <Namespace>DocoptNet</Namespace>
</Query>

void Main()
{
    
    // Usage: serve [<path>] [-port PORT]
    
    var usage = @"Usage: serve [<path>] [--port=PORT]

-h --help   show this
--port=PORT   https port
";

    //var input = @"";
    //var input = @"C:\Temp";
    //var input = @"--port=1234";
    //var input = @"C:\Temp --port=1234";
    //var input = @"agfasdga dasdg asdgasdg";
    //var input = @"-h";

    var args = input.Split(' ');

    var arguments = new Docopt().Apply(usage, args, version: "serve 0.0.1", exit: true);
    
    arguments.Dump();
}

// Define other methods and classes here