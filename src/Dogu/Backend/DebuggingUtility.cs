using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dogu.Backend
{
    public static class DebuggingUtility
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions =
            new JsonSerializerOptions {WriteIndented = true};

        static DebuggingUtility()
        {
            _jsonSerializerOptions.Converters.Add(new TypeJsonConverter());
            _jsonSerializerOptions.Converters.Add(new PropertyInfoJsonConverter());
            _jsonSerializerOptions.Converters.Add(new MethodInfoJsonConverter());
            _jsonSerializerOptions.Converters.Add(new ParameterInfoJsonConverter());
        }

        public static string Serialize(object @object)
        {
            string json = JsonSerializer.Serialize(@object, _jsonSerializerOptions);

            return json;
        }
    }

    public class TypeJsonConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.FullName);
        }
    }

    public class PropertyInfoJsonConverter : JsonConverter<PropertyInfo>
    {
        public override PropertyInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, PropertyInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }

    public class MethodInfoJsonConverter : JsonConverter<MethodInfo>
    {
        public override MethodInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, MethodInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }

    public class ParameterInfoJsonConverter : JsonConverter<ParameterInfo>
    {
        public override ParameterInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, ParameterInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
