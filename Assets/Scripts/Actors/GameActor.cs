using System;
using Environment;
using Interfaces;
using Structs;
using UnityEngine;

namespace Actors
{
    public abstract class GameActor : MonoBehaviour, ISScrollable
    {
        public Effect effect;
        public int lane;

        public abstract void Move(float speed);

        public Effect GetEffect()
        {
            return effect;
        }
        
        private void OnEnable()
        {
            SubscribeMoveEvent(Move);
        }

        protected abstract void SubscribeMoveEvent(Scroller.MoveSubsriber move);


        private void OnDisable()
        {
            UnSubscribeMoveEvent(Move);
        }

        protected abstract void UnSubscribeMoveEvent(Scroller.MoveSubsriber move);
    }
}