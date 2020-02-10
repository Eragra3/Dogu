using System;
using System.Reflection;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Property
    {
        public Type           RawType                { get; set; }
        public PropertyInfo   RawPropertyInfo        { get; set; }
        public string         Name                   { get; set; }
        public string         Type                   { get; set; }
        public AccessModifier PropertyAccessModifier { get; set; }

        public bool            HasSetter            { get; set; }
        public AccessModifier? SetterAccessModifier { get; set; }
        public MethodInfo?     RawSetMethod         { get; set; }
        public Method?         SetMethod            { get; set; }

        public bool            HasGetter            { get; set; }
        public AccessModifier? GetterAccessModifier { get; set; }
        public MethodInfo?     RawGetMethod         { get; set; }
        public Method?         GetMethod            { get; set; }

        public Property(PropertyParameters parameters)
        {
            RawType                = parameters.RawType;
            RawPropertyInfo        = parameters.RawPropertyInfo;
            Name                   = parameters.Name;
            Type                   = parameters.Type;
            PropertyAccessModifier = parameters.PropertyAccessModifier;
            HasSetter              = parameters.HasSetter;
            SetterAccessModifier   = parameters.SetterAccessModifier;
            RawSetMethod           = parameters.RawSetMethod;
            SetMethod              = parameters.SetMethod;
            HasGetter              = parameters.HasGetter;
            GetterAccessModifier   = parameters.GetterAccessModifier;
            RawGetMethod           = parameters.RawGetMethod;
            GetMethod              = parameters.GetMethod;
        }

        public override string ToString() => DebuggingUtility.Serialize(this);
    }
}
