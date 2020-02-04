using Interfaces;
using Rewrite.Enums;
using Rewrite.GameObjects.MainCharacter;
using Rewrite.Handlers;
using Rewrite.Spawner;
using Rewrite.UI;
using Rewrite.Utility;
using UnityEngine;
using EventHandler = Rewrite.Handlers.EventHandler;

namespace Rewrite.GameObjects.Actors
{
    public class Animal: FGameObject, IKillable, ISlowable, IPushable
    {
        private bool _bLerp, _alreadyKilled;
        protected bool Leaping;

        public EnemyType type;

        private float _lerpFactor, _slowAmount;
        public float baseSpeed;
        protected float Speed, CreationTime, SpeedDeviancy;
        
        public int score;
        private int _pushcount;
        public int maxPushes;
        public int staminaScalePerPush;
        public int staminaAmount;
        
        public SpriteRenderer[] renderers;
        public ParticleSystem killParticleSpawner;
        public GameObject bloodPile;
        public Animator animator;
        public GameObject scoreFloater;
        public RectTransform scoreParent;

        private Vector3 _target, _start;
        
        public float leapingDuration;


        protected void Start()
        {
            CreationTime = Time.time;
            Speed = baseSpeed;
            SetupParticleSystemPlanes();
            SetSpriteRendererSortingLayers();
            NpcSpawner.AddEnemyToField();
            scoreParent = SceneObjectsHandler.Handler.scoreParent;
        }
        
        protected void Update()
        {
            if (_bLerp)
            {
                _lerpFactor += Time.deltaTime/leapingDuration;
                Vector3 niew = Vector3.Lerp(_start, _target, _lerpFactor);
                transform.position = niew;
                if (_lerpFactor >= 1)
                {
                    _lerpFactor = 0;
                    _bLerp = false;
                    Leaping = false;
                }
            }
            if(transform.position.x <= -ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + SceneObjectsHandler.Handler.playerObject.xPositioning - 1) EventHandler.UnSubscribePushEvent(Push);
        }

        private void SetSpriteRendererSortingLayers()
        {
            foreach (SpriteRenderer renderer in renderers) renderer.sortingLayerName = "Lane" + (Lane + 1);
        }

        private void SetupParticleSystemPlanes()
        {
            if (killParticleSpawner)
                killParticleSpawner.collision.SetPlane(0, LaneManager.Manager.particleDespawnerPlane[Lane]);
            if (killParticleSpawner)
                killParticleSpawner.subEmitters.GetSubEmitterSystem(0)
                    .collision.SetPlane(0, LaneManager.Manager.particleDespawnerPlane[Lane]);
        }

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

        public override void Move(float speed)
        {
            if (!Leaping)
            {
                Transform trans = transform;
                Vector3 pos = trans.position;
                float moveSpeed = (((Speed + SpeedDeviancy) * (1 - _slowAmount)) - speed) * Time.deltaTime;
                trans.position = new Vector3(pos.x + moveSpeed, pos.y, pos.z);
                if (transform.position.x <= LaneManager.Manager.DespawnXPos)
                {
                    Kill(NpcSpawner.Spawner.gameObject);
                    //ScoreHandler.Handler.ResetCombo();
                }
            }
        }

        public override void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
            if (lane == Lane)
            {
                SpeedDeviancy = deviancy;
            }
        }

        public virtual bool Kill(GameObject killer)
        {
            if (!killer.gameObject.GetComponent<MovementHandler>() && !_alreadyKilled)
            {
                if(killParticleSpawner && !killParticleSpawner.isPlaying)killParticleSpawner.Play(true);
                EventHandler.UnSubscribePushEvent(Push);
                GetComponent<Collider2D>().enabled = false;
                DisableAllRenderers();
                SpawnBloodPile();
                SpawnScoreText();
                _alreadyKilled = true;
                Destroy(gameObject, killParticleSpawner?killParticleSpawner.main.duration:0);
                NpcSpawner.RemoveEnemyFromField();
            } 
            else if (!_alreadyKilled)
            {
                Destroy(gameObject);
                NpcSpawner.RemoveEnemyFromField();
            }
            return true;
        }

        private void SpawnScoreText()
        {
            GameObject instance = Instantiate(scoreFloater, SceneObjectsHandler.Handler.mainCamera.WorldToScreenPoint(transform.position), Quaternion.identity, scoreParent.transform);
            instance.GetComponent<ScoreFloating>().start =
                SceneObjectsHandler.Handler.mainCamera.WorldToScreenPoint(transform.position);
        }

        private void SpawnBloodPile()
        {
            Instantiate(bloodPile, gameObject.transform.position, Quaternion.identity);
        }

        private void DisableAllRenderers()
        {
            foreach (Renderer renderer in renderers) renderer.enabled = false;
        }

        public void StartSlow(float amount)
        {
            _slowAmount = amount / 100;
        }

        public void EndSlow()
        {
            _slowAmount = 0;
        }

        public virtual bool Push(int lane, float distance)
        {
            if (lane == Lane && _pushcount < maxPushes)
            {
                Leaping = true;
                Vector3 pos = transform.position;
                _start = pos;
                _target = new Vector3(pos.x + distance, pos.y);
                _bLerp = true;
                PlayLeapAnimation();
                _pushcount++;
                return true;
            }

            return false;
        }
        
        public int GetStamina()
        {
            return staminaAmount + _pushcount * staminaScalePerPush;
        }
        
        public int GetScore()
        {
            return score;
        }

        private void PlayLeapAnimation()
        {
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            float rightScreenX = ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera);
            float distance = (rightScreenX - 0.5f) - transform.position.x;
            if (other.gameObject.CompareTag("Player") && other.GetComponent<Wolf>().Lane == Lane && !_bLerp) { EventHandler.OnPushEvent(Lane, distance);}
        }
        
    }
}