using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmiumOS.Core
{
    public class Command
    {
        public readonly string name;
        public readonly string desc;

        public Command(string name, string desc)
        {
            this.name = name;
            this.desc = desc;
        }

        public virtual void execute(string[] args) { }
    }
}
