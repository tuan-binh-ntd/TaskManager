using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TaskManager.Core.Extensions
{
    public static partial class CoreExtensions
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [GeneratedRegex("^\\w|_\\w")]
        private static partial Regex MyRegex();

        /// <summary>
        /// Convert json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T FromJson<T>(this string json)
        {
            T val = JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
            if (val == null)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new(25, 1);
                defaultInterpolatedStringHandler.AppendLiteral("Can't Deserialize object ");
                defaultInterpolatedStringHandler.AppendFormatted(typeof(T));
                throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
            }

            return val;
        }

        /// <summary>
        /// Convert object to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }

        public static string ToPascalCase(this string name)
        {
            return MyRegex().Replace(name, (match) => match.Value.Replace("_", "").ToUpper());
        }

    }
}
