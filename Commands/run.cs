using System;
using OsmiumOS.Core;

namespace OsmiumOS.Commands
{
    public class run : Command
    {
        public run(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            OsmiumScript.Run(args[1]);
        }
    }
}
