using System;
using Dogu.Assembly;

namespace Dogu.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("You have to provide assembly path");
            }

            string assemblyPath = args[0];

            var reader = new AssemblyReader(assemblyPath);

            foreach (var type in reader.DefinedTypes)
            {
                System.Console.WriteLine(type.FullName);
            }

            System.Console.WriteLine("Finishing Dogu");
        }
    }
}
