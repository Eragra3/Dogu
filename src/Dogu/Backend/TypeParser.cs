using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dogu.Backend.Structures;
using Enum = Dogu.Backend.Structures.Enum;

namespace Dogu.Backend
{
    public class TypeParser
    {
        private IAssemblyReader _assemblyReader;

        public TypeParser(IAssemblyReader assemblyReader)
        {
            _assemblyReader = assemblyReader;
        }

        public IList<TopLevelType> Parse()
        {
            var types = _assemblyReader.GetExportedTypes();

            var codeMembers = types
                .Select(type =>
                {
                    TopLevelTypeEnum elementTypeEnum = MapTypeToTopLevelType(type);

                    TopLevelType element = elementTypeEnum switch
                    {
                        TopLevelTypeEnum.Class => ParseClass(type),
                        TopLevelTypeEnum.Interface => ParseInterface(type),
                        TopLevelTypeEnum.Enum => ParseEnum(type),
                        TopLevelTypeEnum.Structure => ParseStructure(type),
                        TopLevelTypeEnum.Event => throw new NotImplementedException(),
                        TopLevelTypeEnum.Delegate => throw new NotImplementedException(),
                        _ => throw new ArgumentOutOfRangeException(
                            $"Got code type element that cannot be exported on assembly level, '{elementTypeEnum}'")
                    };

                    return element;
                })
                .ToList();

            return codeMembers;
        }

        protected virtual Structure ParseStructure(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly)
                .Select(ParseMethod)
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(type);

            var structure = new Structure(type, type.FullName, name, accessModifier, methods);

            return structure;
        }

        protected virtual Class ParseClass(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly
                            | BindingFlags.NonPublic)
                // IsSpecialName excludes properties
                .Where(x => (x.IsPublic || x.IsFamily) && !x.IsSpecialName)
                .Select(ParseMethod)
                .ToArray();

            Indexer[] indexers = type
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Static
                               | BindingFlags.Public
                               | BindingFlags.DeclaredOnly
                               | BindingFlags.NonPublic)
                .Where(x => x.GetIndexParameters().Any() &&
                            new[] {x.GetMethod, x.SetMethod}.Any(y => y != null && (y.IsPublic || y.IsFamily)))
                .Select(ParseIndexer)
                .ToArray();

            Property[] properties = type
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Static
                               | BindingFlags.Public
                               | BindingFlags.DeclaredOnly
                               | BindingFlags.NonPublic)
                // Exclude indexers by examining `GetIndexParameters`
                .Where(x => !x.GetIndexParameters().Any() &&
                            new[] {x.GetMethod, x.SetMethod}.Any(y => y != null && (y.IsPublic || y.IsFamily)))
                .Select(ParseProperty)
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(type);

            var @class = new Class(type, type.FullName, name, accessModifier, methods, properties, indexers);

            return @class;
        }

        protected virtual Interface ParseInterface(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly)
                // IsSpecialName excludes properties
                .Where(x => !x.IsSpecialName)
                .Select(ParseMethod)
                .ToArray();

            Indexer[] indexers = type
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Static
                               | BindingFlags.Public
                               | BindingFlags.DeclaredOnly
                               | BindingFlags.NonPublic)
                .Where(x => x.GetIndexParameters().Any() &&
                            new[] {x.GetMethod, x.SetMethod}.Any(y => y != null && (y.IsPublic || y.IsFamily)))
                .Select(ParseIndexer)
                .ToArray();

            Property[] properties = type
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Public
                               | BindingFlags.DeclaredOnly
                               | BindingFlags.NonPublic)
                // Exclude indexers
                .Where(x => !x.GetIndexParameters().Any())
                .Select(ParseProperty)
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(type);

            var @interface = new Interface(type, type.FullName, name, accessModifier, methods, properties, indexers);

            return @interface;
        }

        protected virtual Enum ParseEnum(Type type)
        {
            var enumNames = type.GetEnumNames();
            //TODO: support for enums other than int
            var enumValues = type.GetEnumValues().Cast<int>().Select(x => x.ToString()).ToArray();

            var zipped = enumNames.Zip(enumValues);

            IDictionary<string, string> values = zipped
                .ToDictionary<(string, string), string, string>(pair => pair.Item1, pair => pair.Item2);

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            var @enum = new Enum(type, type.FullName, type.Name, accessModifier, values);

            return @enum;
        }

        protected virtual Property ParseProperty(PropertyInfo propertyInfo)
        {
            var property = new Property(propertyInfo);

            return property;
        }

        protected virtual Method ParseMethod(MethodInfo methodInfo)
        {
            var method = new Method(methodInfo);

            return method;
        }


        protected virtual Indexer ParseIndexer(PropertyInfo propertyInfo)
        {
            var indexer = new Indexer(propertyInfo);

            return indexer;
        }

        protected TopLevelTypeEnum MapTypeToTopLevelType(Type type)
        {
            return type switch
            {
                var t when t.IsClass => TopLevelTypeEnum.Class,
                var t when t.IsInterface => TopLevelTypeEnum.Interface,
                var t when t.IsEnum => TopLevelTypeEnum.Enum,
                //TODO: var t when t.IsClass => TopLevelTypeEnum.Delegate,
                //TODO: var t when t.IsClass => TopLevelTypeEnum.Event,
                var t when t.IsValueType => TopLevelTypeEnum.Structure,
                _ => throw new InvalidOperationException(
                    $"Given type cannot be mapped to any {nameof(TopLevelTypeEnum)}")
            };
        }
    }
}
