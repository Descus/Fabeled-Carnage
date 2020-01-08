using System;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct BackgroundTile
    {
        [SerializeField] private string Name;
        public GameObject tile;
    }
}