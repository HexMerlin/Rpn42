

using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Custom JSON converter for the <see cref="SavedData"/> class.
/// </summary>
public class SavedDataJsonConverter : JsonConverter<SavedData>
{
    /// <summary>
    /// Reads JSON and converts it to a <see cref="SavedData"/> object.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>The deserialized <see cref="SavedData"/> object.</returns>
    public override SavedData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token");

        var savedData = new SavedData();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read(); // Move to property value

                if (propertyName == nameof(SavedData.numberEntries))
                {
                    var entries = new System.Collections.Generic.List<NumberEntry>();

                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read(); // Move to first value in the inner array (Numerator)
                                BigInteger numerator = BigInteger.Parse(reader.GetString());

                                reader.Read(); // Move to second value in the inner array (Denominator)
                                BigInteger denominator = BigInteger.Parse(reader.GetString());

                                reader.Read(); // Move past EndArray

                                entries.Add(new NumberEntry(new Q(numerator, denominator)));
                            }
                        }
                    }

                    savedData.numberEntries = entries.ToArray();
                }
                else if (propertyName == nameof(SavedData.input))
                {
                    savedData.input = reader.GetString();
                }
            }
        }

        return savedData;
    }

    /// <summary>
    /// Writes a <see cref="SavedData"/> object to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The <see cref="SavedData"/> object to write.</param>
    /// <param name="options">The JSON serializer options.</param>
    public override void Write(Utf8JsonWriter writer, SavedData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(SavedData.numberEntries));
        writer.WriteStartArray();

        foreach (var entry in value.numberEntries)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(entry.Q.Numerator.ToString());
            writer.WriteStringValue(entry.Q.Denominator.ToString());
            writer.WriteEndArray();
        }

        writer.WriteEndArray();

        writer.WriteString(nameof(SavedData.input), value.input);

        writer.WriteEndObject();
    }
}
