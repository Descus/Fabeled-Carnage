using System;
using UnityEngine;

public abstract class Animal : MonoBehaviour, IsMovable
{
        private float _moveSpeedModifier;
        protected bool stunned = false;
        public bool Stunned
        {
                get => stunned;
                set => stunned = value;
        }
        
        public abstract void Move(float speed);

        void OnTriggerEnter2D(Collider2D other)
        {
                Debug.Log("geht");
        
        
                if (other.gameObject.CompareTag("Player"))
                {
                        Leap();
                }
        }

        protected abstract void Leap();
}
