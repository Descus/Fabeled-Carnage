using UnityEngine;

public class ScreenUtil
{
        public static float getRightScreenBorderX(Camera main)
        {
                return main.orthographicSize * getScreenRatio();
        }

        public static float getScreenRatio()
        {
                return Screen.width / (float) Screen.height;
        }
}
