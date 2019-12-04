using UnityEngine;

namespace Utility
{
    public class ScreenUtil
    {
        public static float GetRightScreenBorderX(Camera main)
        {
            return main.orthographicSize * GetScreenRatio();
        }

        public static float GetScreenRatio()
        {
            return Screen.width / (float) Screen.height;
        }
    }
}