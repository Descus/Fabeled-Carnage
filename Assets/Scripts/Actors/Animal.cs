using Environment;
using Interfaces;
using UnityEngine;
using Utility;

namespace Actors
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Animal : GameActor, IKillable, ISlowable, IPushable
    {
        //Privates
        private bool _bLerp;
        private float _lerpfac;
        public bool leaping;
        public float slowAmount;
        public float speedMult;

        private NpcSpawner spawner;

        [SerializeField] private int staminaAmount = 25;

        void Start()
        {
            spawner = GameObject.Find("Scroller").GetComponent<NpcSpawner>();
        }
        
        public void Kill()
        {
            Destroy(gameObject);
            NpcSpawner.RemoveEnemy();
        }

        public void StartSlow(float amount)
        {
            slowAmount = amount / 100;
        }

        public void EndSlow()
        {
            slowAmount = 0;
        }

        public override void Move(float speed)
        {
            if (!leaping)
            {
                Transform transform1 = transform;
                Vector3 pos = transform1.position;
                float moveSpeed = speed + speedMult * (1 - slowAmount) * Time.deltaTime;
                transform1.position = new Vector3(pos.x + moveSpeed, pos.y, pos.z);
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
            if(transform.position.x <= spawner.xPositioning) EventHandler.UnSubscribePushEvent(Push);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            float rightSreenX = ScreenUtil.GetRightScreenBorderX(cam.GetComponent<Camera>());
            float distance = (rightSreenX - 0.5f) - transform.position.x;
            if (other.gameObject.CompareTag("Player") && !_bLerp) EventHandler.OnPushEvent(lane, distance);
        }

        
        public void Stun(float time)
        {
        }

        public int GetStamina()
        {
            return staminaAmount;
        }

        protected abstract void PlayLeapAnim();

        protected override void SubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.SubscribeActorMoveEvent(move);
        }

        protected override void UnSubscribeMoveEvent(EventHandler.MoveSubsriber move)
        {
            EventHandler.UnSubscribeActorMoveEvent(move);
        }

        private new void OnEnable()
        {
            EventHandler.SubscribePushEvent(Push);
            base.OnEnable();
        }

        private new void OnDisable()
        {
            EventHandler.UnSubscribePushEvent(Push);
            base.OnDisable();
        }

#pragma warning disable 649
        private Vector3 _target;
        private Vector3 _start;
#pragma warning restore 649
        
        public void Push(int lane, float distance)
        {
            if (lane == this.lane)
            {
                leaping = true;
                Vector3 pos = transform.position;
                _start = pos;
                _target = new Vector3(pos.x + distance, pos.y);
                _bLerp = true;
                PlayLeapAnim();
            }
        }
    }
}