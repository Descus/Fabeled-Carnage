using Interfaces;
using Rewrite.Spawner;
using UnityEngine;

namespace Rewrite.GameObjects.Background
{
    public class Background: MonoBehaviour, IScrollable
    {
        public void Move(float speed)
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            trans.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if(transform.position.x <= BackgroundSpawner.spawner.despawnXPos) Destroy(gameObject);
        }
        public void SetSpeedDeviancyforLane(float deviancy, int lane) { }

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