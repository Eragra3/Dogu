using System;
using System.Drawing;
using Dogu.Backend;
using Dogu.Backend.Structures;
using Colorful;

namespace Dogu.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            args = new[] {@"Dogu.dll"};
#endif
            if (args.Length < 1)
            {
                throw new Exception("You have to provide assembly path");
            }

            string assemblyPath = args[0];

            var reader = new AssemblyReader(assemblyPath);

            foreach (var type in reader.GetExportedTypes())
            {
                // System.Console.WriteLine(type.FullName);
            }

            var parser = new TypeParser(new AssemblyReader(assemblyPath));

            foreach (TopLevelType type in parser.Parse())
            {
                Colorful.Console.WriteLine(type, Color.Gray);
            }

            Colorful.Console.WriteLine("Finishing Dogu", Color.Green);
        }
    }
}
