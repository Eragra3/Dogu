using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Structure : TopLevelType
    {
        public readonly Method[] Methods;

        public Structure(Type rawType, string fullName, string name, AccessModifier accessModifier, Method[] methods) :
            base(rawType, fullName, name, accessModifier)
        {
            Methods = methods;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            string methodsString = string.Join($"{Environment.NewLine}    -", Methods.Select(x => x.ToString()));

            sb.AppendLine(base.ToString());
            sb.Append($"  {nameof(Methods)}");
            if (string.IsNullOrEmpty(methodsString))
            {
                sb.Append($"=<NONE>");
            }
            else
            {
                sb.Append($"{Environment.NewLine}    -{methodsString}");
            }

            return sb.ToString();
        }
    }
}
