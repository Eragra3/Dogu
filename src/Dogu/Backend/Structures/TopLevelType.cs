using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Dogu.Backend.Structures.Parameters;

namespace Dogu.Backend.Structures
{
    public abstract class TopLevelType
    {
        public Type           RawType        { get; }
        public string         FullName       { get; }
        public string         Name           { get; }
        public AccessModifier AccessModifier { get; }

        protected TopLevelType(TopLevelTypeParameters parameters)
        {
            RawType        = parameters.RawType;
            Name           = parameters.Name;
            FullName       = parameters.FullName;
            AccessModifier = parameters.AccessModifier;
        }

        public override string ToString() => DebuggingUtility.Serialize(this);
    }
}
