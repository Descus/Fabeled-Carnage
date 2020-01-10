using Interfaces;
using UnityEngine;

namespace Actors.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class Grass : Obstacle, IKillable
    {
        public float slowAmount;

        public bool Kill(GameObject killer)
        {
            Destroy(gameObject);
            return true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<ISlowable>() != null) other.GetComponent<ISlowable>().StartSlow(slowAmount);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<ISlowable>() != null) other.GetComponent<ISlowable>().EndSlow();
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Lane" + (lane + 1);
        }
    }
}