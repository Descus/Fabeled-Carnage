using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct Effect
    {
#pragma warning disable 649
        [SerializeField] private EffectTypes type;
        [SerializeField] private int duration;
        [SerializeField] private int amount;
#pragma warning restore 649
        
        public int Amount => amount;
        public int Duration => duration;
        public EffectTypes Type => type;
        
    }
}