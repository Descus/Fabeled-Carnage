using System;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Structs
{
    [Serializable]
    public struct Effect
    {
        [SerializeField] private EffectTypes type;
        [SerializeField] private int duration;
        [SerializeField] private int amount;
        
        public int Amount => amount;
        public int Duration => duration;
        public EffectTypes Type => type;
        
    }
}