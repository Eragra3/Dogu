using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public abstract class TopLevelType
    {
        public string FullName;
        public string Name;

        protected TopLevelType(string fullName, string name)
        {
            Name = name;
            FullName = fullName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetType().FullName);
            sb.AppendLine($"  {nameof(FullName)}={FullName}");
            sb.Append($"  {nameof(Name)}={Name}");

            return sb.ToString();
        }
    }
}
