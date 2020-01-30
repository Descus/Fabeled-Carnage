using Interfaces;
using UnityEngine;

namespace Rewrite.GameObjects
{
    public abstract class FGameObject: MonoBehaviour, IScrollable
    {
        private int _lane;

        public int Lane
        {
            get => _lane;
            set => _lane = value;
        }

        protected void OnEnable()
        {
            SubscribeMoveEvent(Move);
        }

        protected void OnDisable()
        {
            UnSubscribeMoveEvent(Move);
        }
        
        protected abstract void SubscribeMoveEvent(EventHandler.MoveSubsriber move);
        protected abstract void UnSubscribeMoveEvent(EventHandler.MoveSubsriber move);
        
        public abstract void Move(float speed);
        public abstract void SetSpeedDeviancyforLane(float deviancy, int lane);
    }
}