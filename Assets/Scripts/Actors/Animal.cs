using System.Numerics;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;
using Vector3 = UnityEngine.Vector3;

namespace Actors
{
        public abstract class Animal : MonoBehaviour, ISMovable
        {
                public bool leaping;
                private bool _bLerp;
#pragma warning disable 649
                private Vector3 _target;
                private Vector3 _start;
#pragma warning restore 649
                private float _lerpfac;

                public float speedMult = 1.0f;
                [SerializeField]
                private int staminaAmount = 25;

                public void Move(float speed)
                {
                        if (!leaping)
                        {
                                Transform transform1 = transform;
                                Vector3 pos = transform1.position;
                                transform1.position = new Vector3(pos.x - (speed * 0.8f) * speedMult, pos.y, pos.z);
                        }
                }

                void Update()
                {

                        
                        if (_bLerp)
                        {
                                _lerpfac += Time.deltaTime;
                                Vector3 niew = Vector3.Lerp(_start, _target, _lerpfac);
                                transform.position = niew;
                                if (_lerpfac >= 1)
                                {
                                        _lerpfac = 0;
                                        _bLerp = false;
                                        leaping = false;
                                }
                        }
                }

                void OnTriggerEnter2D(Collider2D other)
                {
                        if (other.gameObject.CompareTag("Player") && !_bLerp)
                        {
                                Leap();
                        }
                }

                private void Leap()
                {
                        leaping = true;
                        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                        float rightSreenX = ScreenUtil.GetRightScreenBorderX(cam.GetComponent<Camera>());
                        Vector3 pos = transform.position;
                        _start = pos;
                        _target = new Vector3(rightSreenX - 0.5f, pos.y);
                        _bLerp = true;
                        PlayLeapAnim();
                }

                protected abstract void PlayLeapAnim();

                public void Stun(float time)
                {
                        
                }

                public abstract void GetEffect();

                public int GetStamina()
                {
                        return staminaAmount;
                }

                public void Kill()
                {
                        Destroy(this);
                }
        }
}
