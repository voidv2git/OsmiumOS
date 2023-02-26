using System;
using System.IO;
using OsmiumOS.Core;

namespace OsmiumOS.Commands
{
    public class edit : Command
    {
        string text = "";

        public edit(string name, string desc) : base(name, desc) { }

        public override void execute(string[] args)
        {
            try
            {
                string text = File.ReadAllText(args[1]);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.Write(text);

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    text += '\n';
                }
                else if (key.Key == ConsoleKey.Backspace && text.Length > 0)
                {
                    text = text[0..^1];
                }
                else if (key.Key == ConsoleKey.Tab)
                {
                    text += "    ";
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    File.WriteAllText(args[1], text);
                    Console.Clear();
                    return;
                }
                else
                {
                    text += key.KeyChar;
                }
            }
        }
    }
}