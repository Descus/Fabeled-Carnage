using System;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class ColorPrefab
    {
        public Color color;
        public GameObject prefab;

        public bool CompareColors(Color b)
        {
            Debug.Log(b.r * 1000 + " " + color.r * 1000);
            Debug.Log(Mathf.RoundToInt(b.r * 1000) + " " + Mathf.RoundToInt(color.r * 1000));
            return Mathf.RoundToInt(b.r * 1000)  == Mathf.RoundToInt(color.r * 1000)
                   &&  Mathf.RoundToInt( b.g * 1000) == Mathf.RoundToInt(color.g * 1000)
                   && Mathf.RoundToInt(b.b * 1000) == Mathf.RoundToInt(color.b * 1000);

        }
    }
}