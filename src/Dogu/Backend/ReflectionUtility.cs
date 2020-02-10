using System;
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
    }
}
