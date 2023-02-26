using System;
using System.ComponentModel;
using System.IO;
using Sys = Cosmos.System;

namespace OsmiumOS.Core
{
    public class Kernel : Sys.Kernel
    {
        CommandManager commandManager = new CommandManager();
        string logo = @"
  # # # # #
 # 76      #
 #   OS    #
 # Osmium  #
 #         #
  # # # # #
";

        protected override void BeforeRun()
        {
            Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(logo);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Welcome to ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("OsmiumOS\n");
        }

        protected override void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("admin");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("@OsmiumOS > ");
            Console.ResetColor();

            string cmd = Console.ReadLine();
            commandManager.Execute(cmd);
        }
    }
}
