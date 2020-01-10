using System;
using Actors.MainCharacter;
using UnityEngine;

namespace Actors.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class Stone : Obstacle
    {
        public float staminaLoss;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Wolf>().ReduceStamina(staminaLoss);
            }
        }
    }
}