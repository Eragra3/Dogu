using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Class : TopLevelType
    {
        public readonly Method[] Methods;

        public Class(string fullName, string name, Method[] methods) : base(fullName, name)
        {
            Methods = methods;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(base.ToString());
            sb.AppendLine($"  {nameof(Methods)}");
            sb.Append($"    -{string.Join($"{Environment.NewLine}    -", Methods.Select(x => x.ToString()))}");

            return sb.ToString();
        }
    }
}
