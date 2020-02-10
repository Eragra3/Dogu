using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Method
    {
        public readonly string Name;
        public readonly Type ReturnTypeRaw;
        public readonly string ReturnType;
        public readonly AccessModifier AccessModifier;
        public readonly Parameter[] Parameters;

        public Method(MethodInfo methodInfo)
        {
            Name = methodInfo.Name;
            ReturnTypeRaw = methodInfo.ReturnType;
            ReturnType = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(methodInfo.ReturnType);
            AccessModifier = ReflectionUtility.GetAccessModifier(methodInfo);

            Parameter[] parameters = methodInfo
                .GetParameters()
                .Select(y => new Parameter(y.ParameterType, y))
                .ToArray();
            Parameters = parameters;
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters.Select(x => x.ToString()));

            string result = $"{Name}({parameters}) -> {ReturnType}";

            return result;
        }
    }
}
