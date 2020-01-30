using Actors;
using Rewrite.Spawner;
using UnityEngine;

namespace Rewrite.GameObjects.Actors.Ambient
{
    public class BloodEffects: FGameObject
    {
        protected override void SubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.SubscribeBackgroundMoveEvent(move);
        }

        protected override void UnSubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.UnSubscribeBackgroundMoveEvent(move);
        }

        public override void Move(float speed)
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            trans.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if(transform.position.x <= BackgroundSpawner.spawner.despawnXPos) Destroy(gameObject);
        }

        public override void SetSpeedDeviancyforLane(float deviancy, int lane) { }
    }
}