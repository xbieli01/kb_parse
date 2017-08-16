using System;
using System.Collections.Generic;
using System.Linq;

namespace KbParser.Dto
{
    /// <summary>
    /// trieda zapuzdrujuca kolekcie DTO objektov
    /// </summary>
    /// <typeparam name="T">typ prvku v kolekcii</typeparam>
    public class EnumerableDto<T>
    {
        /// <summary>
        /// kolekcia zaznamov v poli
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// vytvori novu prazdnu instanciu 
        /// </summary>
        public static EnumerableDto<T> Empty
        {
            get
            {
                return new EnumerableDto<T>
                {
                    Items = new List<T>()
                };
            }
        }

        /// <summary>
        /// Zistenie, ci kolekcia obsahuje nejake polozky
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Items != null && Items.Any();
        }

        /// <summary>
        /// Zistenie, ci kolekcia obsahuje nejake polozky
        /// </summary>
        /// <returns></returns>
        public bool Any(Func<T, bool> predicate)
        {
            return Items != null && Items.Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool All(Func<T, bool> predicate)
        {
            return Items != null && Items.All(predicate);
        }

        /// <summary>
        /// Vrati prvy element z kolekcie
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            return Items.First();
        }

        public bool HasItems { get { return !IsEmpty; } }

        public bool IsEmpty { get { return Items == null || Items.Count == 0; } }

        public int Count { get { return Items == null ? 0 : Items.Count; } }
    }
}
