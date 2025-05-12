using System;
using System.Collections.Generic;

namespace Common.ListEx
{
    public static class ListExtensions
    {
        /// <summary>
        /// string을 enum으로 변환 함수
        /// </summary>
        public static bool TryGetTuple<TKey, TValue>(this List<(TKey, TValue)> list, TKey target, out (TKey, TValue) result)
        {
            foreach (var item in list)
            {
                if (item.Item1 == null)
                    throw new Exception("List<(TKey, TValue)> list.Item1 is None.");

                if (item.Item1.Equals(target))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
