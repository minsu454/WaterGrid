using UnityEngine;

namespace Common.PhysicsEx
{
    public static class PhysicsEx
    {
        /// <summary>
        /// 두 원의 충돌 여부를 판단 함수
        /// </summary>
        public static bool IsCircleOverlapping(Vector2 posA, float radiusA, Vector2 posB, float radiusB)
        {
            float distance = Vector2.Distance(posA, posB);
            return distance < (radiusA + radiusB);
        }

        /// <summary>
        /// 원과 점의 충돌 여부를 판단 함수
        /// </summary>
        public static bool IsPointInCircle(Vector2 circle, float radius, Vector2 point)
        {
            float distance = Vector2.Distance(circle, point);
            return distance < radius;
        }
    }
}

