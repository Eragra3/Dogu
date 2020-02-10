using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Method
    {
        public string         Name           { get; }
        public Type           RawReturnType  { get; }
        public string         ReturnType     { get; }
        public AccessModifier AccessModifier { get; }
        public Parameter[]    Parameters     { get; }

        public Method(MethodParameters parameters)
        {
            Name           = parameters.Name;
            RawReturnType  = parameters.RawReturnType;
            ReturnType     = parameters.ReturnType;
            AccessModifier = parameters.AccessModifier;
            Parameters     = parameters.Parameters;
        }

        public override string ToString() => DebuggingUtility.Serialize(this);
    }
}
