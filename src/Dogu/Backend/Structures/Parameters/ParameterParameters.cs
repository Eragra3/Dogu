using System;
using System.Reflection;

namespace Dogu.Backend.Structures.Parameters
{
    public class ParameterParameters
    {
        public string        Name             { get; set; }
        public Type          RawType          { get; set; }
        public ParameterInfo RawParameterInfo { get; set; }
        public string        Type             { get; set; }
        public bool          IsOut            { get; set; }
        public bool          IsIn             { get; set; }
        public bool          IsOptional       { get; set; }
        public bool          IsRef            { get; set; }
        public string?       DefaultValue     { get; set; }
    }
}
