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
        public readonly bool HasGetter;
        public readonly AccessModifier? GetterAccessModifier;

        public Property(PropertyInfo propertyInfo)
        {
            RawType = propertyInfo.PropertyType;
            RawPropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            Type = ReflectionUtility.GenerateCodeMarkupForGeneratedTypeName(RawType);

            HasSetter = propertyInfo.CanWrite;
            if (HasSetter)
            {
                SetterAccessModifier = ReflectionUtility.GetAccessModifier(propertyInfo.SetMethod);
            }

            HasGetter = propertyInfo.CanRead;
            if (HasGetter)
            {
                GetterAccessModifier = ReflectionUtility.GetAccessModifier(propertyInfo.GetMethod);
            }

            PropertyAccessModifier =
                GetterAccessModifier > SetterAccessModifier ? GetterAccessModifier : SetterAccessModifier;
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
