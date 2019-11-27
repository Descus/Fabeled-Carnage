using UnityEngine;

public class ScreenUtil
{
        public static float getRightScreenBorderX(Camera main)
        {
                return main.orthographicSize * getScreenRatio().width;
        }

        public static Resolution getScreenRatio()
        {
                int gcd = ScreenUtil.gcd(Screen.width, Screen.height);
                Resolution ratio = new Resolution();
                ratio.height = Screen.height / gcd;
                ratio.width = Screen.width / gcd;
                return ratio;
        }

        private static int gcd(int width, int height)
        {
                return (height == 0) ? width : gcd(height, width % height);
        }
}
