using Interfaces;
using UnityEngine;

namespace Environment
{
    public class Background : MonoBehaviour, ISScrollable
    {
        public void Move(float speed)
        {
            Transform transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x + speed, pos.y, pos.z);
            if (transform.position.x <= -BackgroundSpawner.SpawnX)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnEnable()
        {
            Scroller.SubscribeBackgroundMoveEvent(Move);
        }

        private void OnDisable()
        {
            Scroller.UnSubscribeBackgroundMoveEvent(Move);
        }
    }
}
