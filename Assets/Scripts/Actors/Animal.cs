using System;
using Environment;
using Interfaces;
using UnityEngine;
using Utility;
using EventHandler = Environment.EventHandler;

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
        public float baseSpeed = 1.1f;
        protected float Speed;
        protected float TimeCreation;
        public int score;
        private int _pushcount;
        public int maxPusches = 1;
        public int staminaScalePerPush;

        public SpriteRenderer[] renderers;

        public Animator animator;
        
        protected float SpeedDeviancy = 0;

        private NpcSpawner _spawner;

        [SerializeField] private int staminaAmount = 25;

        protected void Start()
        {
            _spawner = GameObject.Find("Spawner").GetComponent<NpcSpawner>();
            TimeCreation = Time.time;
            Speed = Mathf.Abs(baseSpeed);

            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.sortingLayerName = "Lane" + (lane + 1);
            }
        }
        
        public virtual bool Kill(GameObject killer)
        {
            Destroy(gameObject);
            NpcSpawner.RemoveEnemy();
            return true;
        }

        public void StartSlow(float amount)
        {
            slowAmount = amount / 100;
        }

        public void EndSlow()
        {
            slowAmount = 0;
        }

        public override void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
            if (lane == this.lane)
            {
                SpeedDeviancy = deviancy;
            }
        }

        public override void Move(float speed)
        {
            if (!leaping)
            {
                Transform transform1 = transform;
                Vector3 pos = transform1.position;
                float moveSpeed = (speed - (Speed + SpeedDeviancy) * (1 - slowAmount) ) * Time.deltaTime;
                transform1.position = new Vector3(pos.x + moveSpeed, pos.y, pos.z);
                if (transform.position.x <= -LaneManager.Spawnx)
                {
                    Kill(_spawner.gameObject);
                    ScoreHandler.Handler.ResetCombo();
                }
            }
        }

        protected void Update()
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
            if(transform.position.x <= -NpcSpawner.RightSreenX + _spawner.xPositioning - 1) EventHandler.UnSubscribePushEvent(Push);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            float rightSreenX = ScreenUtil.GetRightScreenBorderX(cam.GetComponent<Camera>());
            float distance = (rightSreenX - 0.5f) - transform.position.x;
            if (other.gameObject.CompareTag("Player") && !_bLerp) EventHandler.OnPushEvent(lane, distance);
        }

        public int GetStamina()
        {
            return staminaAmount + _pushcount * staminaScalePerPush;
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
            EventHandler.SubscribeSpeedDeviancyEvent(SetSpeedDeviancyforLane);
            base.OnEnable();
        }

        private new void OnDisable()
        {
            EventHandler.UnSubscribePushEvent(Push);
            EventHandler.UnSubscribeSpeedDeviancyEvent(SetSpeedDeviancyforLane);
            base.OnDisable();
        }

#pragma warning disable 649
        private Vector3 _target;
        private Vector3 _start;
        
#pragma warning restore 649
        
        public virtual bool Push(int lane, float distance)
        {
            if (lane == this.lane && _pushcount < maxPusches)
            {
                leaping = true;
                Vector3 pos = transform.position;
                _start = pos;
                _target = new Vector3(pos.x + distance, pos.y);
                _bLerp = true;
                PlayLeapAnim();
                _pushcount++;
                return true;
            }

            return false;
        }

        public int GetScore()
        {
            return score;
        }
    }
}