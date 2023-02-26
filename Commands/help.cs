using System;
using OsmiumOS.Core;

namespace OsmiumOS.Commands
{
    public class help : Command
    {
        public help(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            CommandManager commandManager = new CommandManager();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OsmiumOS Help Menu");
            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (Command cmd in commandManager.commands)
            {
                Console.WriteLine(cmd.name + "   " + cmd.desc);
            }
        }
    }
}
