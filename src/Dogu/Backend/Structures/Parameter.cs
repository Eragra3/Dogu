using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Parameter
    {
        public readonly string Name;
        public readonly Type RawType;
        public readonly ParameterInfo RawParameterInfo;
        public readonly string Type;
        public readonly bool IsOut;
        public readonly bool IsIn;
        public readonly bool IsOptional;
        public readonly bool IsRef;
        public readonly string? DefaultValue;

        public Parameter(string name, Type type, ParameterInfo parameterInfo)
        {
            Name = name;
            RawType = type;
            Type = ReflectionUtility.GeneratedTypeToCodeMarkup(type);
            RawParameterInfo = parameterInfo;
            IsOut = parameterInfo.IsOut;
            IsIn = parameterInfo.IsIn;
            IsOptional = parameterInfo.IsOptional;
            IsRef = !IsOut && type.IsByRef;
            DefaultValue = parameterInfo.DefaultValue?.ToString() ?? null;
        }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}
