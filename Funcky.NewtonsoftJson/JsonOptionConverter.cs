using System;
using System.Reflection;
using Funcky.Monads;
using Newtonsoft.Json;

namespace Funcky.NewtonsoftJson
{
    internal sealed class JsonOptionConverter : JsonConverter
    {
        private static readonly MethodInfo GenericWriteMethod
            = typeof(JsonOptionConverter)
                .GetMethod(nameof(WriteJson), BindingFlags.Static | BindingFlags.NonPublic)!;

        private static readonly MethodInfo GenericReadMethod
            = typeof(JsonOptionConverter)
                .GetMethod(nameof(ReadJson), BindingFlags.Static | BindingFlags.NonPublic)!;

        public override bool CanConvert(Type objectType)
            => objectType.IsGenericType && typeof(Option<>).IsAssignableFrom(objectType.GetGenericTypeDefinition());

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var type = value.GetType();

            if (!CanConvert(type))
            {
                throw new JsonSerializationException($"Expected an Option<>, but got a '{type}' instead.");
            }

            var writeMethod = GenericWriteMethod.MakeGenericMethod(type.GetGenericArguments());
            writeMethod.Invoke(null, new[] { writer, value, serializer });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var readMethod = GenericReadMethod.MakeGenericMethod(objectType.GetGenericArguments());
            return readMethod.Invoke(null, new object[] { reader, serializer });
        }

        private static void WriteJson<TItem>(JsonWriter writer, Option<TItem> option, JsonSerializer serializer)
            where TItem : notnull
            => option.Match(
                some: item => serializer.Serialize(writer, item),
                none: writer.WriteNull);

        private static Option<TItem> ReadJson<TItem>(JsonReader reader, JsonSerializer serializer)
            where TItem : notnull
            => reader.TokenType == JsonToken.Null
                ? Option<TItem>.None()
                : Option.Some(serializer.Deserialize<TItem>(reader)!);
    }
}
