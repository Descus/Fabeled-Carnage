using System;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct BackgroundTile
    {
        [SerializeField] private String Name;
        public GameObject tile;
    }
}