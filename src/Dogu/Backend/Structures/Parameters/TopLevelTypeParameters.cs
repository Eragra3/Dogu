using System;

namespace Dogu.Backend.Structures.Parameters
{
    public class TopLevelTypeParameters : SerializableParameters
    {
        public Type           RawType           { get; set; }
        public string         FullName       { get; set; }
        public string         Name           { get; set; }
        public AccessModifier AccessModifier { get; set; }
    }
}
