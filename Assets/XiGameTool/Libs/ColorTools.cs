
using UnityEngine;

namespace XiArtManager.Libs
{
    public static class ColorTools
    {
        public static Color Parse(string color)
        {
            Color outColor = Color.black;
            if (ColorUtility.TryParseHtmlString(color, out outColor))
                return outColor;
            Debug.LogErrorFormat("Can't parse color '{0}'", color);
            return outColor;
        }

    }
}
