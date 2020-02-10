using System;
using Dogu.Backend;

namespace Dogu.Frontend.Common
{
    public static class CodePrinter
    {
        private const string UnsupportedKeywordPrinterErrorMessage = "Value {0} is not one of supported keywords in {1} method";

        public static string PrintAccessModifier(AccessModifier accessModifier)
        {
            return accessModifier switch
            {
                AccessModifier.Private           => "private",
                AccessModifier.Protected         => "protected",
                AccessModifier.ProtectedInternal => "protected internal",
                AccessModifier.Internal          => "internal",
                AccessModifier.Public            => "public",
                _ => throw new InvalidOperationException(
                    string.Format(UnsupportedKeywordPrinterErrorMessage, accessModifier, nameof(PrintAccessModifier)))
            };
        }

        public static string SimplifyLibraryTypes(string fullTypeName)
        {
            return fullTypeName switch
            {
                "System.Object"  => "object",
                "System.String"  => "string",
                "System.Char"    => "char",
                "System.Decimal" => "decimal",
                "System.Double"  => "double",
                "System.Single"  => "float",
                "System.UInt64"  => "ulong",
                "System.UInt32"  => "uint",
                "System.UInt16"  => "ushort",
                "System.Int64"   => "long",
                "System.Int32"   => "int",
                "System.Int16"   => "short",
                "System.Byte"    => "byte",
                "System.SByte"   => "sbyte",
                "System.Boolean" => "bool",
                "System.Void"    => "void",
                _                => fullTypeName
            };
        }
    }
}
