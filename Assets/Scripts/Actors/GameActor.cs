using Environment;
using Interfaces;
using Structs;
using UnityEngine;

namespace Actors
{
    public abstract class GameActor : MonoBehaviour, ISScrollable
    {
        public Effect effect;

        public abstract void Move(float speed);

        public Effect GetEffect()
        {
            return effect;
        }

        private void OnEnable()
        {
            Scroller.SubscribeMoveEvent(Move);
        }

        private void OnDisable()
        {
            Scroller.UnSubscribeMoveEvent(Move);
        }
    }
}