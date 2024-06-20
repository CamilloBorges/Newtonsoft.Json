using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
    #if (NET6_0_OR_GREATER)
    public class IsoDateOnlyConverter : DateOnlyConverterBase
    {
        private const string DefaultDateOnlyFormat = "yyyy'-'MM'-'dd";
        private string? _dateOnlyFormat;
        private CultureInfo? _culture;

        /// <summary>
        /// Gets or sets the date format used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date format used when converting a date to and from JSON.</value>
        public string? DateOnlyFormat
        {
            get => _dateOnlyFormat ?? string.Empty;
            set => _dateOnlyFormat = (StringUtils.IsNullOrEmpty(value)) ? null : value;
        }

        /// <summary>
        /// Gets or sets the culture used when converting a date to and from JSON.
        /// </summary>
        /// <value>The culture used when converting a date to and from JSON.</value>
        public CultureInfo Culture
        {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            string text;

            if (value is DateOnly dateOnly)
            {
                text = dateOnly.ToString(_dateOnlyFormat ?? DefaultDateOnlyFormat, Culture);
            }
            else
            {
                throw new JsonSerializationException("Unexpected value when converting date. Expected DateOnly, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)!));
            }

            writer.WriteValue(text);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            bool nullable = ReflectionUtils.IsNullableType(objectType);
            if (reader.TokenType == JsonToken.Null)
            {
                if (!nullable)
                {
                    throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
                }

                return null;
            }


            if (reader.TokenType == JsonToken.Date)
            {
                return reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
            }

            string? dateText = reader.Value?.ToString();

            if (StringUtils.IsNullOrEmpty(dateText) && nullable)
            {
                return null;
            }

            MiscellaneousUtils.Assert(dateText != null);

            if (!StringUtils.IsNullOrEmpty(_dateOnlyFormat))
            {
                return DateOnly.ParseExact(dateText, _dateOnlyFormat, Culture);
            }
            else
            {
                return DateOnly.Parse(dateText, Culture);
            }
        }
    }
#endif
}
