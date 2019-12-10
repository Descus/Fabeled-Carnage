using Environment;
using Interfaces;
using UnityEngine;
using Utility;

namespace Actors
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Animal : GameActor, ISKillable
    {
        //Privates
        private bool _bLerp;
        private float _lerpfac;
        public bool leaping;
        public float speedMult = 1.0f;

        [SerializeField] private int staminaAmount = 25;

        public void Kill()
        {
            Destroy(gameObject);
            NpcSpawner.RemoveEnemy();
        }

        public override void Move(float speed)
        {
            if (!leaping)
            {
                Transform transform1 = transform;
                Vector3 pos = transform1.position;
                transform1.position = new Vector3(pos.x - speed * 0.8f * speedMult, pos.y, pos.z);
                if (transform.position.x <= -LaneManager.Spawnx)
                {
                    Destroy(gameObject);
                    NpcSpawner.RemoveEnemy();
                }
            }
        }

        private void Update()
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && !_bLerp) Leap();
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

        public void Stun(float time)
        {
        }

        public int GetStamina()
        {
            return staminaAmount;
        }

        protected abstract void PlayLeapAnim();
#pragma warning disable 649
        private Vector3 _target;
        private Vector3 _start;
#pragma warning restore 649
    }
}