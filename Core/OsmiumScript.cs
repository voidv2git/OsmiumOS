using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmiumOS.Core
{
    static class OsmiumScript
    {
        static int pc;
        static string source;
        static Dictionary<string, string[]> variable;

        // Return The Current Valid Character Excusing Any And All Comment Lines
        static char Look()
        {
            if (source[pc] == '#')
            {
                while (source[pc] != '\n' && source[pc] != '\0')
                {
                    pc++;
                }
            }
            return source[pc];
        }

        // Return The Current Character And Increments Program Counter (pc)
        static char Take()
        {
            char c = Look();
            pc++;
            return c;
        }

        // Return Boolean Value Based On If The Next String Is The String Provided
        static bool TakeString(string word)
        {
            int copypc = pc;
            foreach (char c in word)
            {
                if (Take() != c)
                {
                    pc = copypc;
                    return false;
                }
            }
            return true;
        }

        // Returns The Next Non-White-Space Character
        static char Next()
        {
            while (Look() == ' ' || Look() == '\t' || Look() == '\n' || Look() == '\r')
            {
                Take();
            }
            return Look();
        }

        // Returns If A Certain Character Should Be 'eaten'
        static bool TakeNext(char c)
        {
            if (Next() == c)
            {
                Take();
                return true;
            }
            return false;
        }

        // Basic Checks
        static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        static bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }
        static bool IsAlNum(char c)
        {
            return IsDigit(c) || IsAlpha(c);
        }
        static bool IsAddOp(char c)
        {
            return c == '+' || c == '-';
        }
        static bool IsMulOp(char c)
        {
            return c == '*' || c == '/';
        }

        static string TakeNextAlNum()
        {
            string alnum = "";
            if (IsAlpha(Next()))
            {
                while (IsAlNum(Look()))
                {
                    alnum += Take();
                }
            }
            return alnum;
        }

        // --------------------------------------------------------

        static bool BooleanFactor(bool[] act)
        {
            bool inv = TakeNext('!');
            string[] e = expression(act);
            bool b = false;
            Next();

            if (e[0] == "i")
            {
                if (TakeString("==")) b = int.Parse(e[1]) == MathExpression(act);
                else if (TakeString("!=")) b = int.Parse(e[1]) != MathExpression(act);
                else if (TakeString("<=")) b = int.Parse(e[1]) <= MathExpression(act);
                else if (TakeString("<")) b = int.Parse(e[1]) < MathExpression(act);
                else if (TakeString(">=")) b = int.Parse(e[1]) >= MathExpression(act);
                else if (TakeString(">")) b = int.Parse(e[1]) > MathExpression(act);
            }
            else
            {
                if (TakeString("==")) b = e[1] == StringExpression(act);
                else if (TakeString("!=")) b = e[1] != StringExpression(act);
                else b = e[1] != "";
            }
            return act[0] && b != inv;
        }

        static bool BooleanTerm(bool[] act)
        {
            bool b = BooleanFactor(act);
            while (TakeNext('&')) b = b & BooleanFactor(act);
            return b;
        }

        static bool BooleanExpression(bool[] act)
        {
            bool b = BooleanTerm(act);
            while (TakeNext('|')) b = b | BooleanTerm(act);
            return b;
        }

        static int MathFactor(bool[] act)
        {
            int m = 0;
            if (TakeNext('('))
            {
                m = MathExpression(act);
                if (!TakeNext(')')) Error("Missing ')'");
            }
            else if (IsDigit(Next()))
            {
                while (IsDigit(Look())) m = 10 * m + Take() - '0';
            }
            else if (TakeString("val("))
            {
                string s = Str(act);
                if (act[0] && int.TryParse(s, out _)) m = int.Parse(s);
                if (!TakeNext(')')) Error("Missing ')'");
            }
            else
            {
                string ident = TakeNextAlNum();
                if (!variable.ContainsKey(ident) || variable[ident][0] != "i") Error("Unknown Variable -> " + ident);
                else if (act[0]) m = int.Parse(variable[ident][1]);
            }
            return m;
        }

        static int MathTerm(bool[] act)
        {
            int m = MathFactor(act);
            while (IsMulOp(Next()))
            {
                char c = Take();
                int m2 = MathFactor(act);
                if (c == '*') m = m * m2;
                else m = m / m2;
            }
            return m;
        }

        static int MathExpression(bool[] act)
        {
            char c = Next();
            if (IsAddOp(c)) c = Take();
            int m = MathTerm(act);
            if (c == '-') m = -m;
            while (IsAddOp(Next()))
            {
                c = Take();
                int m2 = MathTerm(act);
                if (c == '+') m = m + m2;
                else m = m - m2;
            }
            return m;
        }

        static string Str(bool[] act)
        {
            string s = "";
            if (TakeNext('\"'))
            {
                while (!TakeString("\""))
                {
                    if (Look() == '\0') Error("Unexpected EOF");
                    if (TakeString("\\n")) s += '\n';
                    else s += Take();
                }
            }
            else if (TakeString("str("))
            {
                s = MathExpression(act).ToString();
                if (!TakeNext(')')) Error("Missing ')'");
            }
            else if (TakeString("input()"))
            {
                if (act[0]) s = Console.ReadLine();
            }
            else
            {
                string ident = TakeNextAlNum();
                if (variable.ContainsKey(ident) && variable[ident][0] == "s") s = variable[ident][1];
                else Error("Not A String -> " + ident);
            }
            return s;
        }

        static string StringExpression(bool[] act)
        {
            string s = Str(act);
            while (TakeNext('+')) s += Str(act);
            return s;
        }

        static string[] expression(bool[] act)
        {
            int copypc = pc;
            string ident = TakeNextAlNum();
            pc = copypc;
            if (Next() == '\"' || ident == "str" || ident == "input" || variable.ContainsKey(ident) && variable[ident][0] == "s")
            {
                return new string[] { "s", StringExpression(act) };
            }
            else return new string[] { "i", MathExpression(act).ToString() };
        }

        static void DoWhile(bool[] act)
        {
            bool[] local = { act[0] };
            int pc_while = pc;
            while (BooleanExpression(local))
            {
                Block(local);
                pc = pc_while;
            }
            Block(new bool[] { false });
        }

        static void DoIfElse(bool[] act)
        {
            bool b = BooleanExpression(act);
            if (act[0] && b) Block(act);
            else Block(new bool[] { false });
            Next();
            if (TakeString("else"))
            {
                if (act[0] && !b) Block(act);
                else Block(new bool[] { false });
            }
        }

        static void DoGoTo(bool[] act)
        {
            string ident = TakeNextAlNum();
            if (!variable.ContainsKey(ident) || variable[ident][0] != "p") Error("Unknown Subroutine -> " + ident);
            int ret = pc;
            pc = int.Parse(variable[ident][1]);
            Block(act);
            pc = ret;
        }

        static void DoVoidDef()
        {
            string ident = TakeNextAlNum();
            if (ident == "") Error("Missing Subroutine Identifier");
            variable[ident] = new string[] { "p", pc.ToString() };
            Block(new bool[] { false });
        }

        static void DoAssign(bool[] act)
        {
            string ident = TakeNextAlNum();
            if (!TakeNext('=') || ident == "") Error("Unknown Statement -> " + ident);
            string[] e = expression(act);
            if (act[0] || !variable.ContainsKey(ident)) variable[ident] = e;
        }

        static void DoPrint(bool[] act)
        {
            while (true)
            {
                string[] e = expression(act);
                if (act[0]) Console.Write(e[1]);
                if (!TakeNext(',')) return;
            }
        }

        static void DoBreak(bool[] act)
        {
            if (act[0]) act[0] = false;
        }

        static void Statement(bool[] act)
        {
            if (TakeString("print")) DoPrint(act);
            else if (TakeString("if")) DoIfElse(act);
            else if (TakeString("while")) DoWhile(act);
            else if (TakeString("break")) DoBreak(act);
            else if (TakeString("goto")) DoGoTo(act);
            else if (TakeString("void")) DoVoidDef();
            else DoAssign(act);
        }

        static void Block(bool[] act)
        {
            if (TakeNext('{'))
            {
                while (!TakeNext('}'))
                {
                    Block(act);
                }
            }
            else
            {
                Statement(act);
            }
        }

        static void program()
        {
            bool[] act = { true };
            while (Next() != '\0')
            {
                Block(act);
            }
        }

        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + msg);
            Console.ResetColor();
        }

        // --------------------------------------------------------

        public static void Run(string filename)
        {
            variable = new Dictionary<string, string[]>();
            pc = 0;

            try
            {
                source = File.ReadAllText(filename);
                program();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
