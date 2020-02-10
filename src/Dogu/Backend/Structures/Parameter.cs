using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Parameter
    {
        public string        Name             { get; }
        public Type          RawType          { get; }
        public ParameterInfo RawParameterInfo { get; }
        public string        Type             { get; }
        public bool          IsOut            { get; }
        public bool          IsIn             { get; }
        public bool          IsOptional       { get; }
        public bool          IsRef            { get; }
        public string?       DefaultValue     { get; }

        public Parameter(ParameterParameters parameters)
        {
            Name             = parameters.Name;
            RawType          = parameters.RawType;
            RawParameterInfo = parameters.RawParameterInfo;
            Type             = parameters.Type;
            IsOut            = parameters.IsOut;
            IsIn             = parameters.IsIn;
            IsOptional       = parameters.IsOptional;
            IsRef            = parameters.IsRef;
            DefaultValue     = parameters.DefaultValue;
        }

        public override string ToString() => DebuggingUtility.Serialize(this);
    }
}
