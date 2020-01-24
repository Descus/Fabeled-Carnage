using Environment;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Actors
{
    public class Obstacle : GameActor
    {
        public SpriteRenderer renderer;
        void Start()
        {
            renderer.sortingLayerName = "Lane" + (lane + 1);
        }
        
        public override void Move(float speed)
        {
            Transform transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if (transform.position.x <= -LaneManager.manager.Spawnx) Destroy(gameObject);
        }

        public override void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
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