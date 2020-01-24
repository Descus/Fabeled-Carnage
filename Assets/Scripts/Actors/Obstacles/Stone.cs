using System;
using Actors.MainCharacter;
using Environment;
using UnityEngine;

namespace Actors.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class Stone : Obstacle
    {
        public float staminaLoss;
        public float stunLength;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Wolf>().ReduceStamina(staminaLoss);
                other.GetComponent<Wolf>().Stun(stunLength);
                ScoreHandler.Handler.ResetCombo();
            }
        }
    }
}