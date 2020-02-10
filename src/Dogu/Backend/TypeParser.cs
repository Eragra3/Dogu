using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dogu.Backend.Structures;
using Dogu.Backend.Structures.Parameters;
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
                        TopLevelTypeEnum.Class     => ParseClass(type),
                        TopLevelTypeEnum.Interface => ParseInterface(type),
                        TopLevelTypeEnum.Enum      => ParseEnum(type),
                        TopLevelTypeEnum.Structure => ParseStructure(type),
                        TopLevelTypeEnum.Event     => throw new NotImplementedException(),
                        TopLevelTypeEnum.Delegate  => throw new NotImplementedException(),
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
            Method[] methods = ReflectionUtility.GetMethodsOnType(type).Select(ParseMethod).ToArray();

            Indexer[] indexers = ReflectionUtility.GetIndexersOnType(type).Select(ParseIndexer).ToArray();

            Property[] properties = ReflectionUtility.GetPropertiesOnType(type).Select(ParseProperty).ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GetGeneralizedTypeName(type);

            var parameters = new StructureParameters
            {
                Indexers       = indexers,
                Methods        = methods,
                Name           = name,
                Properties     = properties,
                RawType        = type,
                AccessModifier = accessModifier,
                FullName       = type.FullName
            };

            var structure = new Structure(parameters);

            return structure;
        }

        protected virtual Class ParseClass(Type type)
        {
            Method[] methods = ReflectionUtility.GetMethodsOnType(type).Select(ParseMethod).ToArray();

            Indexer[] indexers = ReflectionUtility.GetIndexersOnType(type).Select(ParseIndexer).ToArray();

            Property[] properties = ReflectionUtility.GetPropertiesOnType(type).Select(ParseProperty).ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GetGeneralizedTypeName(type);

            var parameters = new ClassParameters
            {
                Indexers       = indexers,
                Methods        = methods,
                Name           = name,
                Properties     = properties,
                RawType        = type,
                AccessModifier = accessModifier,
                FullName       = type.FullName
            };

            var @class = new Class(parameters);

            return @class;
        }

        protected virtual Interface ParseInterface(Type type)
        {
            Method[] methods = ReflectionUtility.GetMethodsOnType(type).Select(ParseMethod).ToArray();

            Indexer[] indexers = ReflectionUtility.GetIndexersOnType(type).Select(ParseIndexer).ToArray();

            Property[] properties = ReflectionUtility.GetPropertiesOnType(type).Select(ParseProperty).ToArray();

            AccessModifier accessModifier = ReflectionUtility.GetAccessModifier(type);

            string name = ReflectionUtility.GetGeneralizedTypeName(type);

            var parameters = new InterfaceParameters
            {
                Indexers       = indexers,
                Methods        = methods,
                Name           = name,
                Properties     = properties,
                RawType        = type,
                AccessModifier = accessModifier,
                FullName       = type.FullName
            };

            var @interface = new Interface(parameters);

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

            var parameters = new EnumParameters
            {
                Name           = type.Name,
                Values         = values,
                AccessModifier = accessModifier,
                FullName       = type.FullName,
                RawType        = type
            };

            var @enum = new Enum(parameters);

            return @enum;
        }

        protected virtual Property ParseProperty(PropertyInfo propertyInfo)
        {
            var parameters = GetIndexerIRConstructorParameters(propertyInfo);

            var property = new Property(parameters);

            return property;
        }

        protected virtual Method ParseMethod(MethodInfo methodInfo)
        {
            Parameter[] parameters = methodInfo
                .GetParameters()
                .Select(ParseParameter)
                .ToArray();

            var constructorParameters = new MethodParameters
            {
                Name           = methodInfo.Name,
                RawReturnType  = methodInfo.ReturnType,
                ReturnType     = ReflectionUtility.GetGeneralizedTypeName(methodInfo.ReturnType),
                AccessModifier = ReflectionUtility.GetAccessModifier(methodInfo),
                Parameters     = parameters,
            };

            var method = new Method(constructorParameters);

            return method;
        }

        private Parameter ParseParameter(ParameterInfo parameterInfo)
        {
            string? defaultValue = parameterInfo.HasDefaultValue
                ? parameterInfo.DefaultValue?.ToString() ?? "null"
                : null;

            bool isRef = !parameterInfo.IsOut && parameterInfo.ParameterType.IsByRef;

            var parameters = new ParameterParameters
            {
                Name             = parameterInfo.Name,
                RawType          = parameterInfo.ParameterType,
                RawParameterInfo = parameterInfo,
                Type             = ReflectionUtility.GetGeneralizedTypeName(parameterInfo.ParameterType),
                IsOut            = parameterInfo.IsOut,
                IsIn             = parameterInfo.IsIn,
                IsOptional       = parameterInfo.IsOptional,
                IsRef            = isRef,
                DefaultValue     = defaultValue
            };

            var parameter = new Parameter(parameters);

            return parameter;
        }


        protected virtual Indexer ParseIndexer(PropertyInfo propertyInfo)
        {
            var parameters = GetIndexerIRConstructorParameters(propertyInfo);

            var indexer = new Indexer(parameters);

            return indexer;
        }

        protected virtual IndexerParameters GetIndexerIRConstructorParameters(PropertyInfo propertyInfo)
        {
            (AccessModifier? setterAccessModifier, Method setter) = propertyInfo.SetMethod switch
            {
                { } method => (ReflectionUtility.GetAccessModifier(method), ParseMethod(method)),
                null       => ((AccessModifier?)null, (Method)null)
            };

            (AccessModifier? getterAccessModifier, Method getter) = propertyInfo.GetMethod switch
            {
                { } method => (ReflectionUtility.GetAccessModifier(method), ParseMethod(method)),
                null       => ((AccessModifier?)null, (Method)null)
            };

            AccessModifier propertyAccessModifier =
                (getterAccessModifier ?? 0) > (setterAccessModifier ?? 0) ? getterAccessModifier ?? 0 : setterAccessModifier ?? 0;

            var parameters = new IndexerParameters
            {
                RawType                = propertyInfo.PropertyType,
                RawPropertyInfo        = propertyInfo,
                Name                   = propertyInfo.Name,
                Type                   = ReflectionUtility.GetGeneralizedTypeName(propertyInfo.PropertyType),
                PropertyAccessModifier = propertyAccessModifier,
                HasSetter              = propertyInfo.CanWrite,
                SetterAccessModifier   = setterAccessModifier,
                RawSetMethod           = propertyInfo.SetMethod,
                SetMethod              = setter,
                HasGetter              = propertyInfo.CanRead,
                GetterAccessModifier   = getterAccessModifier,
                RawGetMethod           = propertyInfo.GetMethod,
                GetMethod              = getter,
            };

            return parameters;
        }

        protected TopLevelTypeEnum MapTypeToTopLevelType(Type type)
        {
            return type switch
            {
                var t when t.IsClass     => TopLevelTypeEnum.Class,
                var t when t.IsInterface => TopLevelTypeEnum.Interface,
                var t when t.IsEnum      => TopLevelTypeEnum.Enum,
                //TODO: var t when t.IsClass => TopLevelTypeEnum.Delegate,
                //TODO: var t when t.IsClass => TopLevelTypeEnum.Event,
                var t when t.IsValueType => TopLevelTypeEnum.Structure,
                _ => throw new InvalidOperationException(
                    $"Given type cannot be mapped to any {nameof(TopLevelTypeEnum)}")
            };
        }
    }
}
