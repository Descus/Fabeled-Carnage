using System;
using UnityEngine;

namespace Rewrite.Spawning
{
    [Serializable]
    public struct ColorPrefab
    {
        public Color color;
        public GameObject prefab;

        public bool CompareColors(Color b)
        { 
            return Mathf.RoundToInt(b.r * 1000) == Mathf.RoundToInt(color.r * 1000)
                   && Mathf.RoundToInt(b.g * 1000) == Mathf.RoundToInt(color.g * 1000)
                   && Mathf.RoundToInt(b.b * 1000) == Mathf.RoundToInt(color.b * 1000);
        }
    }
}