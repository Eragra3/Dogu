using System.Collections.Generic;

namespace Dogu.Backend.Structures
{
    public class Enum : TopLevelType
    {
        //TODO: add type
        public readonly IDictionary<string, string> Values;

        public Enum(string fullName, string name, AccessModifier accessModifier, IDictionary<string, string> values) : base(fullName, name, accessModifier)
        {
            Values = values;
        }
    }
}
