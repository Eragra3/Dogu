using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dogu.Backend.Structures;

namespace Dogu.Backend
{
    public static class ReflectionUtility
    {
        private const string AccessModifierMappingErrorMessage =
            "Couldn't map type '{0}' to any value of {1} enum";

        public static AccessModifier GetAccessModifier(Type type)
        {
            return type switch
            {
                { IsPublic: true }                                         => AccessModifier.Public,
                { IsPublic: false, IsNested: false }                       => AccessModifier.Internal,
                { IsPublic: false, IsNested: true, IsNestedFamily: true }  => AccessModifier.Protected,
                { IsPublic: false, IsNested: true, IsNestedPrivate: true } => AccessModifier.Private,
                _ => throw new InvalidOperationException(
                    string.Format(AccessModifierMappingErrorMessage, type.Name, nameof(AccessModifier)))
            };
        }

        public static AccessModifier GetAccessModifier(MethodInfo type)
        {
            return type switch
            {
                { IsPublic: true }           => AccessModifier.Public,
                { IsAssembly: true }         => AccessModifier.Internal,
                { IsFamily: true }           => AccessModifier.Protected,
                { IsPrivate: true }          => AccessModifier.Private,
                { IsFamilyOrAssembly: true } => AccessModifier.ProtectedInternal,
                _ => throw new InvalidOperationException(
                    string.Format(AccessModifierMappingErrorMessage, type.Name, nameof(AccessModifier)))
            };
        }

        /// <summary>
        /// Generates generalized type name
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Code representation</returns>
        public static string GetGeneralizedTypeName(Type type)
        {
            string typeName = StripGeneratedTypeName(type.FullName ?? type.Name);

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Any())
            {
                string genericParameters =
                    $"{string.Join(", ", genericArguments.Select(GetGeneralizedTypeName))}";

                return $"{typeName}<{genericParameters}>";
            }
            else
            {
                return typeName;
            }
        }

        /// <summary>
        /// Removes from the type name any string generated during compilation, including all generic types.
        /// <c>"IList`1[System.String]&amp;"</c> will become just <c>"IList"</c>
        /// </summary>
        /// <param name="name">Generated type name</param>
        /// <returns>Stripped type name</returns>
        public static string StripGeneratedTypeName(string name)
        {
            string result = name;

            // Remove generated generic part
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

        public static IEnumerable<MethodInfo> GetMethodsOnType(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance
                                       | BindingFlags.Public
                                       | BindingFlags.DeclaredOnly
                                       | BindingFlags.NonPublic;

            IEnumerable<MethodInfo> methods = type
                .GetMethods(flags)
                // IsSpecialName excludes properties
                .Where(x => !x.IsSpecialName);

            return methods;
        }

        public static IEnumerable<PropertyInfo> GetIndexersOnType(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance
                                       | BindingFlags.Static
                                       | BindingFlags.Public
                                       | BindingFlags.DeclaredOnly
                                       | BindingFlags.NonPublic;

            var indexers = type
                .GetProperties(flags)
                .Where(property => property.GetIndexParameters().Any());

            return indexers;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOnType(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance
                                       | BindingFlags.Static
                                       | BindingFlags.Public
                                       | BindingFlags.DeclaredOnly
                                       | BindingFlags.NonPublic;

            var properties = type
                .GetProperties(flags)
                // Exclude indexers by examining `GetIndexParameters`
                .Where(x => !x.GetIndexParameters().Any());

            return properties;
        }
    }
}
