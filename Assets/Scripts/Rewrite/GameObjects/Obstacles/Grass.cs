using Interfaces;
using UnityEngine;

namespace Rewrite.GameObjects.Obstacles
{
    [RequireComponent(typeof(Collider2D))]
    public class Grass : Obstacle
    {
        public float slowAmount;

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
            GetComponent<SpriteRenderer>().sortingLayerName = "Lane" + (Lane + 1);
        }
    }
}