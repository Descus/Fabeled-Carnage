using Environment;
using Interfaces;
using UnityEngine;

namespace Actors.Obstacles
{
    public class Grass : Obstacle, ISKillable
    {
        public float slowAmount;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<IsSlowable>() != null)
            {
                other.GetComponent<IsSlowable>().StartSlow(slowAmount);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<IsSlowable>() != null)
            {
                other.GetComponent<IsSlowable>().EndSlow();
            }
        }

        void Start()
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Lane" + (lane + 1);
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}

