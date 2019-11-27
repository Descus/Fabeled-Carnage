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
                if (other.gameObject.CompareTag("Player"))
                {
                        Leap();
                }
        }

        protected void Leap()
        {
                Stunned = true;
                GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                float rightSreenX = ScreenUtil.getRightScreenBorderX(cam.GetComponent<Camera>());
                transform.position = new Vector3(rightSreenX - 0.5f, transform.position.y);
                PlayLeapAnim();
                Stunned = false;
        }

        protected abstract void PlayLeapAnim();
}
