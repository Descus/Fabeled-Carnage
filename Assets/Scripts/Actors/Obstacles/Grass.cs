using Interfaces;
using UnityEngine;

namespace Actors.Obstacles
{
    public class Grass : Obstacle, ISKillable
    {
        public float slowAmount;

        public void Kill()
        {
            Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<IsSlowable>() != null) other.GetComponent<IsSlowable>().StartSlow(slowAmount);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<IsSlowable>() != null) other.GetComponent<IsSlowable>().EndSlow();
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Lane" + (lane + 1);
        }
    }
}