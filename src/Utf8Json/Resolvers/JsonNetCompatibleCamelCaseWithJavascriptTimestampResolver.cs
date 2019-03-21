using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Utf8Json.Resolvers
{
    public class JsonNetCompatibleCamelCaseWithJavascriptTimestampResolver : IJsonFormatterResolver
    {
        public static readonly JsonNetCompatibleCamelCaseWithJavascriptTimestampResolver Instance =
            new JsonNetCompatibleCamelCaseWithJavascriptTimestampResolver();

        private static readonly IJsonFormatter[] Formatters =
        {
            new JavascriptTimestampDateTimeFormatter(),
            new NullableJavascriptTimestampDateTimeFormatter(),
//            new Int64AsStringFormatter(),
//            new NullableInt64AsStringFormatter(),
        };

        private static readonly IJsonFormatterResolver[] Resolvers = new[]
        {
            EnumResolver.UnderlyingValueOfPropertyValueOnly,
            DictionaryKeyCamelCaseDynamicGenericResolver.Instance,
            StandardResolver.ExcludeNullCamelCasePropertyValueByEnumUnderlyingValue
        };

        internal JsonNetCompatibleCamelCaseWithJavascriptTimestampResolver()
        {
        }

        public IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly IJsonFormatter<T> Formatter;

            static FormatterCache()
            {
                foreach (var item in Formatters)
                {
                    foreach (var implInterface in item.GetType().GetTypeInfo().ImplementedInterfaces)
                    {
                        var ti = implInterface.GetTypeInfo();
                        if (ti.IsGenericType && ti.GenericTypeArguments[0] == typeof(T))
                        {
                            Formatter = (IJsonFormatter<T>)item;
                            return;
                        }
                    }
                }

                foreach (var item in Resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        Formatter = f;
                        return;
                    }
                }
            }
        }
    }
}

