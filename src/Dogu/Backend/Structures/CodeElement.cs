using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dogu.Backend.Structures
{
    public abstract class CodeElement
    {
        public string FullName;
        public string Name;

        protected CodeElement(string fullName, string name)
        {
            Name = name;
            FullName = fullName;
        }

        public override string ToString()
        {
            Type thisType = GetType();
            var sb = new StringBuilder();

            sb.AppendLine(thisType.Name);
            foreach (var member in thisType.GetFields().Where(x => x.IsPublic))
            {
                sb.AppendLine($"  {member.Name}={member.GetValue(this)}");
            }

            return sb.ToString();
        }
    }
}
