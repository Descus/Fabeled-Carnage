using Environment;
using UnityEngine;

namespace Actors
{
    public class Obstacle : GameActor
    {
        public override void Move(float speed)
        {
            Transform transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x + speed, pos.y, pos.z);
            if (transform.position.x <= -LaneManager.Spawnx) Destroy(gameObject);
        }

        protected override void SubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.SubscribeBackgroundMoveEvent(move);
        }

        protected override void UnSubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.UnSubscribeBackgroundMoveEvent(move);
        }
    }
}