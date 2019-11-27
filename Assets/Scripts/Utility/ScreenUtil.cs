using UnityEngine;

public class ScreenUtil
{
        public static int getRightScreenBorderX(Camera main)
        {
                //main.orthographicSize0;
                return 0;
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
