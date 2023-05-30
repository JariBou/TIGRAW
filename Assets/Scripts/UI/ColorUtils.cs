using UnityEngine;

namespace UI
{
    public class ColorUtils
    {
        public static Color ColorWithAlpha(Color color, float alpha)
        {
            Color temp = color;
            temp.a = alpha;
            return temp;
        }
    }
}