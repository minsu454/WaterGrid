using UnityEditor;
using UnityEngine;
using UnityEngine.Device;

namespace Common.Hexagon
{
    public static class HexUtility
    {
        /// <summary>
        /// 헥사곤 좌표 월드 좌표로 변환 함수
        /// </summary>
        public static Vector2 HexToWorld2D(int q, int r, float tileSize)
        {
            float width = Mathf.Sqrt(3f) * tileSize;
            float height = 1.5f * tileSize;
            float x = (q + r * 0.5f - r / 2) * width;
            float y = r * height;

            return new Vector2(x, y);
        }

        /// <summary>
        /// 헥사곤 하나를 Scene 뷰에 그리는 함수
        /// </summary>
        public static void DrawHex2D(Vector2 position, float tileSize, bool hasValue, float percent, Color color)
        {
            Vector3[] cornerArr = new Vector3[7];
            for (int i = 0; i < 6; i++)
            {
                float angleDeg = 60 * i - 30;
                float angleRad = Mathf.Deg2Rad * angleDeg;
                cornerArr[i] = new Vector3(
                    position.x + tileSize * Mathf.Cos(angleRad),
                    position.y + tileSize * Mathf.Sin(angleRad),
                    0
                );
            }
            cornerArr[6] = cornerArr[0];

            if (hasValue)
            {
                Handles.color = color;
                Handles.DrawAAConvexPolygon(cornerArr[..6]);

                Handles.color = Color.white;

                if (percent > 0)
                {
                    GUIStyle style = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.LowerCenter,
                        normal = { textColor = Color.black },
                        fontStyle = FontStyle.Normal
                    };

                    Handles.Label(new Vector3(position.x, position.y - 0.25f, 0), $"{percent:F2}%", style);
                }
            }

            Handles.DrawPolyLine(cornerArr);
        }

        /// <summary>
        /// 지정된 point가 해당 헥사곤 안에 포함되는지 판별하는 함수
        /// </summary>
        public static bool ContainHex(Vector2 center, Vector3 point, float tileSize)
        {
            Vector2 local = new Vector2(point.x - center.x, point.y - center.y);
            for (int i = 0; i < 6; i++)
            {
                float angle1 = Mathf.Deg2Rad * (60 * i - 30);
                float angle2 = Mathf.Deg2Rad * (60 * (i + 1) - 30);
                Vector2 p1 = new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * tileSize;
                Vector2 p2 = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * tileSize;
                float sign = (p2.x - p1.x) * (local.y - p1.y) - (p2.y - p1.y) * (local.x - p1.x);
                if (sign < 0) return false;
            }
            return true;
        }
    }
}
