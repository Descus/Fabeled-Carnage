using Interfaces;
using Rewrite.Handlers;
using UnityEngine;

namespace Rewrite.GameObjects.Actors.Ambient
{
    public class Pfosten : MonoBehaviour, IScrollable
    {
        public void Move(float speed)
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            trans.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if (transform.position.x <= LaneManager.Manager.DespawnXPos) Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventHandler.SubscribeBackgroundMoveEvent(Move);
        }

        private void OnDisable()
        {
            EventHandler.UnSubscribeBackgroundMoveEvent(Move);
        }
        
        public void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
        }
    }
}
