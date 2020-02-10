using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Method
    {
        public readonly string Name;
        public readonly Parameter[] Parameters;
        public readonly string ReturnType;

        public Method(string name, string returnType, Parameter[] parameters)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
        }

        public Method(string name, string returnType)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = Array.Empty<Parameter>();
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters.Select(x => x.ToString()));

            string result = $"{Name}({parameters}) -> {ReturnType}";

            return result;
        }
    }
}
