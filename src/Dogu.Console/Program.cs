using System;
using Dogu.Backend;
using Dogu.Backend.Structures;

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

            foreach (var type in reader.ExportedTypes)
            {
                // System.Console.WriteLine(type.FullName);
            }


            var parser = new TypeParser(new AssemblyReader(assemblyPath));

            foreach (CodeElement type in parser.Parse())
            {
                System.Console.WriteLine(type);
            }

            System.Console.WriteLine("Finishing Dogu");
        }
    }
}
