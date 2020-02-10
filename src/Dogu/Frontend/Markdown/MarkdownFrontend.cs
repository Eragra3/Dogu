using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dogu.Backend.Structures;
using Enum = Dogu.Backend.Structures.Enum;

namespace Dogu.Frontend.Markdown
{
    public class MarkdownFrontend
    {
        protected readonly IList<TopLevelType> Types;

        public MarkdownFrontend(IList<TopLevelType> types)
        {
            Types = types;
        }

        public void WriteToFile(string filePath)
        {
            File.WriteAllText(filePath, $"# Cool file {Environment.NewLine}");

            foreach (TopLevelType type in Types)
            {
                string section = type switch
                {
                    Class @class => DocumentClass(@class),
                    Enum @enum => DocumentEnum(@enum),
                    Interface @interface => DocumentInterface(@interface),
                    _ => throw new ArgumentOutOfRangeException(nameof(type))
                };

                File.AppendAllText(filePath, section + Environment.NewLine);
            }
        }

        private string DocumentEnum(Enum @enum)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"## {@enum.Name}");

            // Summary
            sb.AppendLine("Overview");
            sb.AppendLine("```csharp");
            sb.AppendLine($"enum {@enum.Name}");
            sb.AppendLine("{");

            foreach ((string name, string value) in @enum.Values)
            {
                sb.AppendLine($"    {name} = {value},");
            }

            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();

            return sb.ToString();
        }

        private string DocumentInterface(Interface @interface)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"## {@interface.Name}");

            // Summary
            sb.AppendLine("Overview");
            sb.AppendLine("```csharp");
            sb.AppendLine($"interface {@interface.Name}");
            sb.AppendLine("{");

            foreach (Method method in @interface.Methods)
            {
                sb.AppendLine($"    {GetMethodSignature(method)};");
            }

            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();

            foreach (Method method in @interface.Methods)
            {
                sb.AppendLine(DocumentMethod(method));
            }

            return sb.ToString();
        }

        private string DocumentClass(Class @class)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"## {@class.Name}");

            // Summary
            sb.AppendLine("Overview");
            sb.AppendLine("```csharp");
            sb.AppendLine($"class {@class.Name}");
            sb.AppendLine("{");

            foreach (Method method in @class.Methods)
            {
                sb.AppendLine($"    {GetMethodSignature(method)};");
            }

            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();

            foreach (Method method in @class.Methods)
            {
                sb.AppendLine(DocumentMethod(method));
            }

            return sb.ToString();
        }

        private string DocumentMethod(Method method)
        {
            string signature = GetMethodSignature(method);

            string section = $@"### {method.Name}

```csharp
{signature}
```";

            return section;
        }

        private static string GetMethodSignature(Method method)
        {
            string parameters = $"({string.Join(", ", method.Parameters.Select(x => x.ToString()))})";

            return $"{method.ReturnType} {method.Name}{parameters}";
        }
    }
}
