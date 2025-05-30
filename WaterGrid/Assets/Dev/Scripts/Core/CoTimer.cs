using System;
using System.Collections;

namespace Common.Timer
{
    public static class CoTimer
    {
        /// <summary>
        /// delay이후 callback함수 호출해주는 코루틴
        /// </summary>
        public static IEnumerator Timer(float delayTime, Action callback)
        {
            yield return Yield.YieldCache.WaitForSeconds(delayTime);
            callback.Invoke();
        }

        /// <summary>
        /// delay이후 callback함수 호출해주는 루프 코루틴
        /// </summary>
        public static IEnumerator Loop(float delayTime, Action callback)
        {
            while (true)
            {
                yield return Yield.YieldCache.WaitForSeconds(delayTime);
                callback.Invoke();
            }
        }

        /// <summary>
        /// delay이후 callback함수 호출해주는 count만큼 루프 코루틴
        /// </summary>
        public static IEnumerator Loop(int count, float delayTime, Action callback)
        {
            while (count == 0)
            {
                yield return Yield.YieldCache.WaitForSeconds(delayTime);
                callback.Invoke();
                count--;
            }
        }
    }
}