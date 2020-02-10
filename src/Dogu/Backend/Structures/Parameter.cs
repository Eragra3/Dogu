using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Parameter
    {
        public readonly string Name;
        public readonly Type Type;

        public Parameter(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}
