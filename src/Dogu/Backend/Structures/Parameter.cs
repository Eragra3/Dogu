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

        public Parameter(Type type, ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;
            RawType = type;
            Type = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(type);
            RawParameterInfo = parameterInfo;
            IsOut = parameterInfo.IsOut;
            IsIn = parameterInfo.IsIn;
            IsOptional = parameterInfo.IsOptional;
            IsRef = !IsOut && type.IsByRef;
            DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue?.ToString() ?? "null" : null;
        }

        public override string ToString()
        {
            string modifiers = "";
            if (IsIn)
            {
                modifiers += "in ";
            }

            if (IsOut)
            {
                modifiers += "out ";
            }

            if (IsRef)
            {
                modifiers += "ref ";
            }

            string defaultValue = "";
            if (IsOptional)
            {
                defaultValue = $" = {DefaultValue}";
            }

            string signature = $"{modifiers}{Type} {Name}{defaultValue}";

            return signature;
        }
    }
}
