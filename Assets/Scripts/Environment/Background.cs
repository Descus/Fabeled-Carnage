using Interfaces;
using UnityEngine;

namespace Environment
{
    public class Background : MonoBehaviour, IScrollable
    {
        public void Move(float speed)
        {
            Transform transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x + speed, pos.y, pos.z);
            if (transform.position.x <= -BackgroundSpawner.SpawnX) Destroy(gameObject);
        }

        public void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
        }

        private void OnEnable()
        {
            EventHandler.SubscribeBackgroundMoveEvent(Move);
        }

        private void OnDisable()
        {
            EventHandler.UnSubscribeBackgroundMoveEvent(Move);
        }
    }
}