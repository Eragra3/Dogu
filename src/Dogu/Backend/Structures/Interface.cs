﻿using System;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public class Interface : TopLevelType
    {
        public readonly Method[] Methods;

        public Interface(string fullName, string name, Method[] methods) : base(fullName, name)
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
