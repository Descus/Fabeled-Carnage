using Rewrite.GameObjects.MainCharacter;
using Rewrite.Handlers;
using UnityEngine;

namespace Rewrite.GameObjects.Obstacles
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
                Wolf wolf = other.GetComponent<Wolf>();
                if (wolf.Lane == Lane)
                {
                    wolf.ReduceStamina(staminaLoss);
                    wolf.Stun(stunLength);
                    ScoreHandler.Handler.ResetCombo();
                }
            }
        }
    }
}