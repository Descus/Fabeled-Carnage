using Interfaces;
using UnityEngine;
using Utility;

namespace Actors
{
        public abstract class Animal : MonoBehaviour, ISMovable
        {
                private float _moveSpeedModifier;
                public bool stunned;

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
                        stunned = true;
                        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                        float rightSreenX = ScreenUtil.GetRightScreenBorderX(cam.GetComponent<Camera>());
                        Transform transform1 = transform;
                        transform1.position = new Vector3(rightSreenX - 0.5f, transform1.position.y);
                        PlayLeapAnim();
                        stunned = false;
                }

                protected abstract void PlayLeapAnim();
        }
}
