using System;
using System.Drawing;
using Dogu.Backend;
using Dogu.Backend.Structures;
using Colorful;
using Dogu.Frontend.Markdown;

namespace Dogu.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
            {
                args = new[] {@"Dogu.dll", "dogu.md"};
            }
#endif
            if (args.Length < 1)
            {
                throw new Exception("You have to provide assembly path");
            }

            if (args.Length < 2)
            {
                throw new Exception("You have to provide output path");
            }

            string assemblyPath = args[0];
            string outputPath = args[1];

            var parser = new TypeParser(new AssemblyReader(assemblyPath));

            foreach (TopLevelType type in parser.Parse())
            {
                Colorful.Console.WriteLine(type, Color.Gray);
            }

            var frontend = new MarkdownFrontend(parser.Parse());

            frontend.WriteToFile(outputPath);

            Colorful.Console.WriteLine("Finishing Dogu", Color.Green);
        }
    }
}
