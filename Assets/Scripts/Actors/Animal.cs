using System.Numerics;
using Interfaces;
using UnityEngine;
using Utility;
using Vector3 = UnityEngine.Vector3;

namespace Actors
{
        public abstract class Animal : MonoBehaviour, ISMovable
        {
                private float _moveSpeedModifier;
                public bool stunned;
                private bool _bLerp;
#pragma warning disable 649
                private Vector3 _target;
                private Vector3 _start;
#pragma warning restore 649
                private float _lerpfac;
                
                public abstract void Move(float speed);

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
                                        stunned = false;
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
                        stunned = true;
                        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                        float rightSreenX = ScreenUtil.GetRightScreenBorderX(cam.GetComponent<Camera>());
                        Vector3 pos = transform.position;
                        _start = pos;
                        _target = new Vector3(rightSreenX - 0.5f, pos.y);
                        _bLerp = true;
                        PlayLeapAnim();
                }

                protected abstract void PlayLeapAnim();
        }
}
