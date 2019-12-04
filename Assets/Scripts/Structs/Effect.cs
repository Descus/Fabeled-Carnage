using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct Effect
    {
#pragma warning disable 649
#pragma warning restore 649

        [field: SerializeField] public int Amount { get; }

        [field: SerializeField] public int Duration { get; }

        [field: SerializeField] public EffectTypes Type { get; }
    }
}