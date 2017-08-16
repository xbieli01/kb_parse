using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbParser.Dto
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// funkcia prevedie kolekciu na EnumerableDto
        /// </summary>
        /// <typeparam name="T">typ zaznamu v kolekcii</typeparam>
        /// <param name="enumerable">kolekcia na konverziu</param>
        /// <returns>instanciu EnumerableDto</returns>
        public static EnumerableDto<T> ToEnumerableDto<T>(this ICollection<T> enumerable)
        {
            return new EnumerableDto<T>() { Items = enumerable.ToList<T>() };
        }

        /// <summary>
        /// funkcia prevedie enumerable na EnumerableDto
        /// </summary>
        /// <typeparam name="T">typ zaznamu v kolekcii</typeparam>
        /// <param name="enumerable">kolekcia na konverziu</param>
        /// <returns>instanciu EnumerableDto</returns>
        public static EnumerableDto<T> ToEnumerableDto<T>(this IEnumerable<T> enumerable)
        {
            return new EnumerableDto<T>() { Items = enumerable.ToList() };
        }
        

        public static bool IsNullOrEmpty<T>(this EnumerableDto<T> enumerableDto)
        {
            return enumerableDto == null || enumerableDto.IsEmpty;
        }

        /// <summary>
        /// funkcia spoji enumeraciu objektov do jedneho stringu
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Concatenate<TSource>(this IEnumerable<TSource> source, string separator = ",")
        {
            if (source == null || !source.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var s in source)
            {
                if (s == null || string.IsNullOrWhiteSpace(s.ToString()))
                {
                    continue;
                }

                if (!isFirst)
                {
                    sb.Append(separator);
                }
                isFirst = false;

                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}
