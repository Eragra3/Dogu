using System;
using System.Collections.Generic;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public class Enum : TopLevelType
    {
        //TODO: add type
        public IDictionary<string, string> Values { get; }

        public Enum(EnumParameters parameters) : base(parameters)
        {
            Values = parameters.Values;
        }
    }
}
