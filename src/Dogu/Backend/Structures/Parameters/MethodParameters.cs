using System;

namespace Dogu.Backend.Structures.Parameters
{
    public class MethodParameters : SerializableParameters
    {
        public string         Name           { get; set; }
        public Type           RawReturnType  { get; set; }
        public string         ReturnType     { get; set; }
        public AccessModifier AccessModifier { get; set; }
        public Parameter[]    Parameters     { get; set; }
    }
}
