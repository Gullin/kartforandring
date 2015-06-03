using System;
using System.Collections.Generic;
using System.Linq;

namespace kartforandring
{
	public static class UtilityDynamicLinqMultiSorting
	{
        /// <summary>
        /// 1. The sortExpressions is a list of Tuples, the first item of the 
        ///    tuples is the field name,
        ///    the second item of the tuples is the sorting order (asc/desc) case sensitive.
        /// 2. If the field name (case sensitive) provided for sorting does not exist 
        ///    in the object,
        ///    exception is thrown
        /// 3. If a property name shows up more than once in the "sortExpressions", 
        ///    only the first takes effect.
        /// </summary>
        /// <remarks>
        /// Source Code by Dr. Song Li
        /// http://www.codeproject.com/Articles/280952/Multiple-Column-Sorting-by-Field-Names-Using-Linq
        /// licensed under The Code Project Open License (CPOL) http://www.codeproject.com/info/cpol10.aspx
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<T> MultipleSort<T>(this IEnumerable<T> data,
          List<Tuple<string, string>> sortExpressions)
        {
            // No sorting needed
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
            {
                return data;
            }

            // Let us sort it
            IEnumerable<T> query = from item in data select item;
            IOrderedEnumerable<T> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                // We need to keep the loop index, not sure why it is altered by the Linq.
                var index = i;
                Func<T, object> expression = item => item.GetType()
                                .GetProperty(sortExpressions[index].Item1)
                                .GetValue(item, null);

                if (sortExpressions[index].Item2 == "asc")
                {
                    orderedQuery = (index == 0) ? query.OrderBy(expression)
                      : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = (index == 0) ? query.OrderByDescending(expression)
                             : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query;
        }
	}
}