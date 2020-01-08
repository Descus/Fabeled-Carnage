using Environment;
using Interfaces;
using Structs;
using UnityEngine;

namespace Actors
{
    public abstract class GameActor : MonoBehaviour, IScrollable
    {
        public Effect effect;
        public int lane;

        public abstract void Move(float speed);

        public Effect GetEffect()
        {
            return effect;
        }

        protected void OnEnable()
        {
            SubscribeMoveEvent(Move);
        }

        protected abstract void SubscribeMoveEvent(EventHandler.MoveSubsriber move);


        protected void OnDisable()
        {
            UnSubscribeMoveEvent(Move);
        }

        protected abstract void UnSubscribeMoveEvent(EventHandler.MoveSubsriber move);
    }
}