using OsmiumOS.Commands;
using System;
using System.Collections.Generic;

namespace OsmiumOS.Core
{
    public class CommandManager
    {
        public List<Command> commands;

        public CommandManager()
        {
            commands = new List<Command>()
            {
                new help("help", "help | Opens the OsmiumOS help menu."),
                new ls("ls", "ls <path> | Lists all files in a certain directory."),
                new file("file", "file <create/delete> <path> | Create or delete files."),
                new dir("dir", "dir <create/delete> <path> | Create or delete directories."),
                new edit("edit", "edit <path> | Opens the text editor."),
                new run("run", "run <path> | Runs a file of choice."),
                new clear("clear", "Clears the command line.")
            };
        }

        public void Execute(string input)
        {
            string[] args = input.Split(" ");

            foreach (Command cmd in commands)
            {
                if (cmd.name == args[0])
                {
                    cmd.execute(args);
                    return;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Undefined Command Error: Could not find command '" + args[0] + "'. Use the command 'help' for a list of all commands and their use.");
        }
    }
}
