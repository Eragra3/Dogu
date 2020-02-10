using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dogu.Backend;
using Dogu.Backend.Structures;
using Dogu.Frontend.Common;
using Enum = Dogu.Backend.Structures.Enum;
using static Dogu.Frontend.Common.CodePrinter;

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
                    Class @class         => DocumentClass(@class),
                    Enum @enum           => DocumentEnum(@enum),
                    Interface @interface => DocumentInterface(@interface),
                    Structure structure  => DocumentStructure(structure),
                    _ => throw new NotSupportedException(
                        $"Type `{type.GetType().FullName}` is not supported with {nameof(MarkdownFrontend)}")
                };

                File.AppendAllText(filePath, section + Environment.NewLine);
            }
        }

        private string DocumentStructure(Structure structure)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"## {structure.Name}");

            // Summary
            sb.AppendLine("Overview");
            sb.AppendLine("```csharp");
            sb.AppendLine($"{PrintAccessModifier(structure.AccessModifier)} struct {structure.Name}");
            sb.AppendLine("{");

            foreach (Method method in structure.Methods)
            {
                sb.AppendLine($"    {GetMethodSignature(method)};");
            }

            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();

            foreach (Method method in structure.Methods)
            {
                sb.AppendLine(DocumentMethod(method));
            }

            return sb.ToString();
        }

        private string DocumentEnum(Enum @enum)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"## {@enum.Name}");

            // Summary
            sb.AppendLine("Overview");
            sb.AppendLine("```csharp");
            sb.AppendLine($"{PrintAccessModifier(@enum.AccessModifier)} enum {@enum.Name}");
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
            sb.AppendLine($"{PrintAccessModifier(@interface.AccessModifier)} interface {@interface.Name}");
            sb.AppendLine("{");

            sb.AppendLine("    // Indexers");
            foreach (Indexer indexer in @interface.Indexers)
            {
                sb.AppendLine($"    {GetIndexerSignature(indexer)};");
            }

            sb.AppendLine("    // Properties");
            foreach (Property property in @interface.Properties)
            {
                sb.AppendLine($"    {GetPropertySignature(property)};");
            }

            sb.AppendLine("    // Methods");
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
            sb.AppendLine($"{PrintAccessModifier(@class.AccessModifier)} class {@class.Name}");
            sb.AppendLine("{");

            sb.AppendLine("    // Indexers");
            foreach (Indexer indexer in @class.Indexers)
            {
                sb.AppendLine($"    {GetIndexerSignature(indexer)};");
            }

            sb.AppendLine("    // Properties");
            foreach (Property property in @class.Properties)
            {
                sb.AppendLine($"    {GetPropertySignature(property)};");
            }

            sb.AppendLine("    // Methods");
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

        private string GetIndexerSignature(Indexer indexer)
        {
            // TODO: get proper implementation
            string propertySignature = GetPropertySignature(indexer);

            string parameters = "";
            if (indexer.HasSetter)
            {
                parameters =
                    $"({string.Join(", ", indexer.SetMethod.Parameters.Select(GetParameterSignature))})";
            }

            // Represent indexer like a property, but with it's name replaced with `this` and parameters
            return propertySignature.Replace($" {indexer.Name}", $" this[{parameters}]");
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
            string parameters = $"({string.Join(", ", method.Parameters.Select(x => GetParameterSignature(x)))})";

            return $"{PrintAccessModifier(method.AccessModifier)} {SimplifyLibraryTypes(method.ReturnType)} {method.Name}{parameters}";
        }

        private static string GetPropertySignature(Property property)
        {
            string getter = "";
            if (property.HasGetter)
            {
                getter = " get;";
            }

            string setter = "";
            if (property.HasSetter)
            {
                setter = " set;";
            }

            string signature =
                $"{PrintAccessModifier(property.PropertyAccessModifier)} {SimplifyLibraryTypes(property.Type)} {property.Name} {{{getter}{setter} }}";

            return signature;
        }

        private static string GetParameterSignature(Parameter parameter)
        {
            string modifiers = "";
            if (parameter.IsIn)
            {
                modifiers += "in ";
            }

            if (parameter.IsOut)
            {
                modifiers += "out ";
            }

            if (parameter.IsRef)
            {
                modifiers += "ref ";
            }

            string defaultValue = "";
            if (parameter.IsOptional)
            {
                defaultValue = $" = {parameter.DefaultValue}";
            }

            string signature = $"{modifiers}{SimplifyLibraryTypes(parameter.Type)} {parameter.Name}{defaultValue}";

            return signature;
        }
    }
}
