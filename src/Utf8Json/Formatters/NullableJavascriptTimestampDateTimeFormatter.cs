using System;
using System.Collections.Generic;
using System.Text;

namespace Utf8Json.Formatters
{
    public sealed class NullableJavascriptTimestampDateTimeFormatter : IJsonFormatter<DateTime?>, IJsonFormatter
    {
        private readonly JavascriptTimestampDateTimeFormatter _innerFormatter;

        public NullableJavascriptTimestampDateTimeFormatter()
        {
            this._innerFormatter = new JavascriptTimestampDateTimeFormatter();
        }

        public void Serialize(ref JsonWriter writer, DateTime? value, IJsonFormatterResolver formatterResolver)
        {
            if (!value.HasValue)
                writer.WriteNull();
            else
                this._innerFormatter.Serialize(ref writer, value.Value, formatterResolver);
        }

        public DateTime? Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
                return new DateTime?();
            return new DateTime?(this._innerFormatter.Deserialize(ref reader, formatterResolver));
        }
    }
}
