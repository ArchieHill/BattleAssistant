using System;
using System.Reflection;

namespace Battle_Assistant.Helpers
{
    /// <summary>
    /// Enum Helper - Converts strings into the enum value for the given enum
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Converts the string into the enum value if it exists in the enum
        /// </summary>
        /// <typeparam name="TEnum">The enum</typeparam>
        /// <param name="text">The enum value in a string form</param>
        /// <returns>The enum value</returns>
        /// <exception cref="InvalidOperationException">Triggers when an enum isn't inputted</exception>
        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
