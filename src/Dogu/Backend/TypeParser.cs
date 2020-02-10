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
                        // _ => throw new ArgumentOutOfRangeException(
                        //     $"Got code type element that cannot be exported on assembly level, '{elementTypeEnum}'")
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

            string name = ReflectionUtility.GeneratedTypeToCodeMarkup(type);

            var structure = new Structure(type, type.FullName, name, accessModifier, methods);

            return structure;
        }

        protected virtual Interface ParseInterface(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly)
                .Select(ParseMethod)
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GeneratedTypeToCodeMarkup(type);

            var @interface = new Interface(type, type.FullName, name, accessModifier, methods);

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

        protected virtual Class ParseClass(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly
                            | BindingFlags.NonPublic)
                .Where(x => x.IsPublic || x.IsFamily)
                .Select(ParseMethod)
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GeneratedTypeToCodeMarkup(type);

            var @class = new Class(type, type.FullName, name, accessModifier, methods);

            return @class;
        }

        protected virtual Method ParseMethod(MethodInfo x)
        {
            if (x.Name == "BindToName")
            {
                Console.Write("");
            }
            Parameter[] parameters = x
                .GetParameters()
                .Select(y => new Parameter(y.Name, y.ParameterType, y))
                .ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(x);

            var method = new Method(x.Name, x.ReturnType, ReflectionUtility.GeneratedTypeToCodeMarkup(x.ReturnType),
                accessModifier, parameters);
            return method;
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
