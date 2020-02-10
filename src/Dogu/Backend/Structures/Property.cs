using System;
using System.Reflection;

namespace Dogu.Backend.Structures
{
    public class Property
    {
        public readonly Type RawType;
        public readonly PropertyInfo RawPropertyInfo;
        public readonly string Name;
        public readonly string Type;
        public readonly AccessModifier? PropertyAccessModifier;

        public readonly bool HasSetter;
        public readonly AccessModifier? SetterAccessModifier;
        public readonly MethodInfo? RawSetMethod;
        public readonly Method? SetMethod;

        public readonly bool HasGetter;
        public readonly AccessModifier? GetterAccessModifier;
        public readonly MethodInfo? RawGetMethod;
        public readonly Method? GetMethod;

        public Property(PropertyInfo propertyInfo)
        {
            RawType = propertyInfo.PropertyType;
            RawPropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            Type = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(RawType);

            HasSetter = propertyInfo.CanWrite;
            RawSetMethod = propertyInfo.SetMethod;
            if (HasSetter)
            {
                SetMethod = new Method(RawSetMethod);
                SetterAccessModifier = ReflectionUtility.GetAccessModifier(RawSetMethod);
            }

            HasGetter = propertyInfo.CanRead;
            RawGetMethod = propertyInfo.GetMethod;
            if (HasGetter)
            {
                GetMethod = new Method(RawGetMethod);
                GetterAccessModifier = ReflectionUtility.GetAccessModifier(RawGetMethod);
            }

            PropertyAccessModifier =
                (GetterAccessModifier ?? 0) > (SetterAccessModifier ?? 0) ? GetterAccessModifier : SetterAccessModifier;
        }

        public override string ToString()
        {
            string getter = "";
            if (HasGetter)
            {
                getter = $"{GetterAccessModifier.ToString().ToLower()} get; ";
            }

            string setter = "";
            if (HasSetter)
            {
                setter = $"{SetterAccessModifier.ToString().ToLower()} set; ";
            }

            string accessors = "";
            if (HasGetter || HasSetter)
            {
                accessors = $" {{ {getter}{setter}}}";
            }

            return $"{Type} {Name}{accessors}";
        }
    }
}
