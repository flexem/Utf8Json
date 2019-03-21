using System.Reflection;

namespace Utf8Json.Resolvers
{
    public class JsonNetCompatibleCamelCaseResolver : IJsonFormatterResolver
    {
        public static readonly JsonNetCompatibleCamelCaseResolver Instance = new JsonNetCompatibleCamelCaseResolver();

        private static readonly IJsonFormatter[] Formatters =
        {
//            new Int64AsStringFormatter(),
//            new NullableInt64AsStringFormatter(),
        };

        static readonly IJsonFormatterResolver[] Resolvers = new[]
        {
            EnumResolver.UnderlyingValueOfPropertyValueOnly,
            DictionaryKeyCamelCaseDynamicGenericResolver.Instance,
            StandardResolver.ExcludeNullCamelCasePropertyValueByEnumUnderlyingValue
        };

        internal JsonNetCompatibleCamelCaseResolver()
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