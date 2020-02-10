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
                    TopLevelTypeEnum elementTypeEnum = MapTypeToCodeElement(type);

                    TopLevelType element = elementTypeEnum switch
                    {
                        TopLevelTypeEnum.Class => ParseClass(type),
                        TopLevelTypeEnum.Interface => ParseInterface(type),
                        TopLevelTypeEnum.Enum => ParseEnum(type),
                        TopLevelTypeEnum.Structure => throw new NotImplementedException(),
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

        private static Interface ParseInterface(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly)
                .Select(x =>
                {
                    Parameter[] parameters = x
                        .GetParameters()
                        .Select(y => new Parameter(y.Name, y.ParameterType))
                        .ToArray();

                    var method = new Method(x.Name, x.ReturnType, parameters);

                    return method;
                })
                .ToArray();

            var @interface = new Interface(type.FullName, type.Name, methods);

            return @interface;
        }

        private static Enum ParseEnum(Type type)
        {
            var enumNames = type.GetEnumNames();
            //TODO: support for enums other than int
            var enumValues = type.GetEnumValues().Cast<int>().Select(x => x.ToString()).ToArray();

            var zipped = enumNames.Zip(enumValues);

            IDictionary<string, string> values = zipped
                .ToDictionary<(string, string), string, string>(pair => pair.Item1, pair => pair.Item2);

            var @enum = new Enum(type.FullName, type.Name, values);

            return @enum;
        }

        private static Class ParseClass(Type type)
        {
            Method[] methods = type
                .GetMethods(BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.Public
                            | BindingFlags.DeclaredOnly
                            | BindingFlags.NonPublic)
                .Where(x => x.IsPublic || x.IsFamily)
                .Select(x =>
                {
                    Parameter[] parameters = x
                        .GetParameters()
                        .Select(y => new Parameter(y.Name, y.ParameterType))
                        .ToArray();

                    var method = new Method(x.Name, x.ReturnType, parameters);

                    return method;
                })
                .ToArray();

            var @class = new Class(type.FullName, type.Name, methods);

            return @class;
        }

        public TopLevelTypeEnum MapTypeToCodeElement(Type type)
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
