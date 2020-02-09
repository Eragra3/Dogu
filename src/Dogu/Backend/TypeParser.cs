using System;
using System.Collections.Generic;
using System.Linq;
using Dogu.Backend.Structures;
using Enum = Dogu.Backend.Structures.Enum;

namespace Dogu.Backend
{
    public class TypeParser
    {
        private IAssemblyReader _assemblyReader;

        public TypeParser(IAssemblyReader assemblyReader)
        {
            _assemblyReader = assemblyReader;
        }

        public IList<CodeElement> Parse()
        {
            var types = _assemblyReader.ExportedTypes.ToList();

            var codeMembers = types
                .Select(x =>
                {
                    CodeElementType elementType = MapTypeToCodeElement(x);

                    CodeElement element = elementType switch
                    {
                        CodeElementType.Class => new Class(x.FullName, x.Name),
                        CodeElementType.Interface => new Interface(x.FullName, x.Name),
                        CodeElementType.Enum => new Enum(x.FullName, x.Name),
                        CodeElementType.Structure => throw new NotImplementedException(),
                        CodeElementType.Event => throw new NotImplementedException(),
                        CodeElementType.Delegate => throw new NotImplementedException(),
                        _ => throw new ArgumentOutOfRangeException(
                            $"Got code type element that cannot be exported on assembly level, '{elementType}'")
                    };

                    return element;
                })
                .ToList();

            return codeMembers;
        }

        public CodeElementType MapTypeToCodeElement(Type type)
        {
            return type switch
            {
                var t when t.IsClass => CodeElementType.Class,
                var t when t.IsInterface => CodeElementType.Interface,
                var t when t.IsEnum => CodeElementType.Enum,
                _ => throw new InvalidOperationException(
                    $"Given type cannot be mapped to any {nameof(CodeElementType)}")
            };
        }
    }
}
