using System;
using System.IO;
using OsmiumOS.Core;
using Sys = Cosmos.System;

namespace OsmiumOS.Commands
{
    public class file : Command
    {
        public file(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            try
            {
                switch (args[1])
                {
                    case "create":
                        Sys.FileSystem.VFS.VFSManager.CreateFile(args[2]);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully created file " + args[2]);
                        break;
                    case "delete":
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(args[2]);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully deleted file " + args[2]);
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
