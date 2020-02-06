using System;
using Rewrite.Handlers;
using UnityEngine;
using EventHandler = Rewrite.Handlers.EventHandler;

namespace Rewrite.GameObjects.Obstacles
{
    public class Obstacle: FGameObject
    {
        public new SpriteRenderer renderer;
        private void Start()
        {
            renderer.sortingLayerName = "Lane" + (Lane + 1);
        }

        public override void Move(float speed)
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            trans.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if (transform.position.x <= LaneManager.Manager.DespawnXPos) Destroy(gameObject);
        }

        public override void SetSpeedDeviancyforLane(float deviancy, int lane) { }

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