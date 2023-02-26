using System;
using OsmiumOS.Core;
using Sys = Cosmos.System;

namespace OsmiumOS.Commands
{
    public class ls : Command
    {
        public ls(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var dirList = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(args[1]);
            foreach (var dir in dirList)
            {
                Console.WriteLine(dir.mName + "   " + dir.mSize);
            }
        }
    }
}
