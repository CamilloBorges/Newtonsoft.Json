using System;

namespace Newtonsoft.Json.Converters
{
#if (NET6_0_OR_GREATER)
    /// <summary>
    /// Provides a base class for converting a <see cref="DateOnly"/> to and from JSON.
    /// </summary>
    public abstract class DateOnlyConverterBase : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateOnly) || objectType == typeof(DateOnly?))
            {
                return true;
            }

            return false;
        }
    }
#endif
}
