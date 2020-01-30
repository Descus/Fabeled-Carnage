using UnityEngine;

namespace Rewrite.Utility
{
    public static class ScreenUtil
    {
        public static float GetRightScreenBorderX(Camera main)
        {
            return main.orthographicSize * GetScreenRatio();
        }

        private static float GetScreenRatio()
        {
            return Screen.width / (float) Screen.height;
        }
    }
}