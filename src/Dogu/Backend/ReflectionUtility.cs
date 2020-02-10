using System;
using System.Linq;
using System.Reflection;

namespace Dogu.Backend
{
    public static class ReflectionUtility
    {
        public static AccessModifier GetAccessModifier(Type type)
        {
            return type switch
            {
                { IsPublic: true } => AccessModifier.Public,
                { IsPublic: false, IsNested: false } => AccessModifier.Internal,
                { IsPublic: false, IsNested: true, IsNestedFamily: true } => AccessModifier.Protected,
                { IsPublic: false, IsNested: true, IsNestedPrivate: true } => AccessModifier.Private,
            };
        }

        public static AccessModifier GetAccessModifier(MethodInfo type)
        {
            return type switch
            {
                { IsPublic: true } => AccessModifier.Public,
                { IsAssembly: true } => AccessModifier.Internal,
                { IsFamily: true } => AccessModifier.Protected,
                { IsPrivate: true } => AccessModifier.Private,
            };
        }

        /// <summary>
        /// Generates code representation of the name for the type
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Code representation</returns>
        public static string GenerateCodeMarkupForGeneratedTypeName(Type type)
        {
            string typeName = GeneratedToGenericName(type.Name);

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Any())
            {
                string genericParameters =
                    $"{string.Join(", ", genericArguments.Select(GenerateCodeMarkupForGeneratedTypeName))}";

                return $"{typeName}<{genericParameters}>";
            }
            else
            {
                return typeName;
            }

            string GeneratedToGenericName(string name)
            {
                string result = name;

                // Remove generated number of parameters
                int generatedPartIndex = name.IndexOf('`');
                if (generatedPartIndex > 0)
                {
                    result = name.Substring(0, generatedPartIndex);
                }

                // Remove trailing ampersand for by-ref parameters
                if (result.EndsWith('&'))
                {
                    return result.Substring(0, result.Length - 1);
                }

                return result;
            }
        }
    }
}
