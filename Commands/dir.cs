using System;
using OsmiumOS.Core;
using Sys = Cosmos.System;

namespace OsmiumOS.Commands
{
    public class dir : Command
    {
        public dir(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            try
            {
                switch (args[1])
                {
                    case "create":
                        Sys.FileSystem.VFS.VFSManager.CreateDirectory(args[2]);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully created directory " + args[2]);
                        break;
                    case "delete":
                        Sys.FileSystem.VFS.VFSManager.DeleteDirectory(args[2], true);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully deleted directory " + args[2]);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Syntax Error: '" + args[1] + "'. Use the command 'help' for help.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
