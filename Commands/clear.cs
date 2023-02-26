using System;
using OsmiumOS.Core;

namespace OsmiumOS.Commands
{
    public class clear : Command
    {
        public clear(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            Console.Clear();
        }
    }
}
