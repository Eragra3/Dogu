using System;
using System.Reflection;

namespace Dogu.Backend.Structures.Parameters
{
    public class PropertyParameters : SerializableParameters
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
    }
}
