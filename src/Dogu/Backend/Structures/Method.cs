using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Method
    {
        public readonly string Name;
        public readonly Parameter[] Parameters;
        public readonly Type ReturnTypeRaw;
        public readonly string ReturnType;
        public readonly AccessModifier AccessModifier;

        public Method(string name, Type returnTypeRaw, string returnType, AccessModifier accessModifier,
            Parameter[] parameters)
        {
            Name = name;
            Parameters = parameters;
            AccessModifier = accessModifier;
            ReturnTypeRaw = returnTypeRaw;
            ReturnType = returnType;
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters.Select(x => x.ToString()));

            string result = $"{Name}({parameters}) -> {ReturnType}";

            return result;
        }
    }
}
