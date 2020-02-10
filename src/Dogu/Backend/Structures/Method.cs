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

        public Method(string name, Type returnTypeRaw, AccessModifier accessModifier, Parameter[] parameters)
        {
            Name = name;
            Parameters = parameters;
            AccessModifier = accessModifier;
            ReturnTypeRaw = returnTypeRaw;
            ReturnType = GenerateHumanReturnType(returnTypeRaw);
        }

        //TODO: nested generic types
        //TODO: extract to reuse it elsewhere
        private static string GenerateHumanReturnType(Type returnTypeRaw)
        {
            if (returnTypeRaw.IsGenericType)
            {
                string baseType = GeneratedToGenericName(returnTypeRaw.Name);
                string genericParameters =
                    $"{string.Join(", ", returnTypeRaw.GetGenericArguments().Select(x => x.Name))}";

                return $"{baseType}<{genericParameters}>";
            }
            else
            {
                return returnTypeRaw.Name;
            }

            string GeneratedToGenericName(string name)
            {
                int generatedPartIndex = name.IndexOf('`');
                return name.Substring(0, generatedPartIndex);
            }
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters.Select(x => x.ToString()));

            string result = $"{Name}({parameters}) -> {ReturnType}";

            return result;
        }
    }
}
