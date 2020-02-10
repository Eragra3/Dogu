using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public abstract class TopLevelType
    {
        public readonly string FullName;
        public readonly string Name;
        public readonly AccessModifier AccessModifier;

        protected TopLevelType(string fullName, string name, AccessModifier accessModifier)
        {
            Name = name;
            FullName = fullName;
            AccessModifier = accessModifier;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetType().FullName);
            sb.AppendLine($"  {nameof(FullName)}={FullName}");
            sb.AppendLine($"  {nameof(Name)}={Name}");
            sb.Append($"  {nameof(AccessModifier)}={AccessModifier}");

            return sb.ToString();
        }
    }
}
