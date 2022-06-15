using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlyingFigures.Model.Converter
{
    public class FigureConverter : JsonConverter<Figure>
    {
        public override Figure? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                switch (jsonDoc.RootElement.GetProperty("Type").GetString())
                {
                    case nameof(Rectangle):
                        return jsonDoc.RootElement.Deserialize<Rectangle>(options);
                    case nameof(Triangle):
                        return jsonDoc.RootElement.Deserialize<Triangle>(options);
                    case nameof(Circle):
                        return jsonDoc.RootElement.Deserialize<Circle>(options);
                    default:
                        throw new JsonException("'Type' doesn't match a known derived type");
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, Figure value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
