using System;
using System.Linq;
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

        public static System.Nullable<Kek<System.Text.Json.Serialization.JsonConverter<System.Nullable<System.Int32>>>> kek  => null;
        public static Kek<System.Text.Json.Serialization.JsonConverter<System.Int32?>>?                                 kek2 => null;

        public static string SimplifyLibraryTypes(string fullTypeName)
        {
            // Change `System.Nullable<>` into `?`
            string simplifiedType = ReplaceNullableWithQuestionMark(fullTypeName);

            simplifiedType = simplifiedType
                .Replace("System.Object", "object")
                .Replace("System.String", "string")
                .Replace("System.Char", "char")
                .Replace("System.Decimal", "decimal")
                .Replace("System.Double", "double")
                .Replace("System.Single", "float")
                .Replace("System.UInt64", "ulong")
                .Replace("System.UInt32", "uint")
                .Replace("System.UInt16", "ushort")
                .Replace("System.Int64", "long")
                .Replace("System.Int32", "int")
                .Replace("System.Int16", "short")
                .Replace("System.Byte", "byte")
                .Replace("System.SByte", "sbyte")
                .Replace("System.Boolean", "bool")
                .Replace("System.Void", "void");

            return simplifiedType;
        }

        public static string ReplaceNullableWithQuestionMark(string fullTypeName)
        {
            const string nullableLeftPart  = "System.Nullable<";
            const string nullableRightPart = ">";

            string result = fullTypeName;

            int nullableTypeIndex = fullTypeName.IndexOf(nullableLeftPart);

            while (nullableTypeIndex > -1)
            {
                int rightAngleBracketIndex = fullTypeName.IndexOf(nullableRightPart, nullableTypeIndex);
                int innerTypeIndex         = nullableTypeIndex + nullableLeftPart.Length;

                result = fullTypeName.Substring(0, nullableTypeIndex)
                         + fullTypeName.Substring(innerTypeIndex, rightAngleBracketIndex - innerTypeIndex)
                         + "?"
                         + fullTypeName.Substring(rightAngleBracketIndex + 1, fullTypeName.Length - rightAngleBracketIndex - 1);

                nullableTypeIndex = fullTypeName.IndexOf(result);
            }

            return result;
        }
    }

    public struct Kek<T>
    {
    }
}
