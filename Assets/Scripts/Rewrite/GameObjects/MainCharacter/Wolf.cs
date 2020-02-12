using Interfaces;
using Rewrite.Enums;
using Rewrite.GameObjects.Actors;
using Rewrite.Handlers;
using Rewrite.UI;
using Rewrite.Utility;
using Rewrite.Scoreboards;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Rewrite.GameObjects.MainCharacter
{
    public class Wolf: MonoBehaviour, ISlowable
    {
        public float slowAmount;
        public float speed;
        public float maxStamina;
        public float xPositioning;

        private bool _attacking, _hasAttacked, _topZone, _botZone, _attackZone;
        private float _changedStaminaMultiplier, _stunEnd, _stamina, _startAttack;
        private int _killsOfSameType;
        private EnemyType _lastKillType;

        public Killzone killzone;
        public Image staminaBar, furyBar;
        public Animator animator;
        public Transform stopperTop, stopperBottom;
        public GameObject uiScreen, gameOverScreen;
        public SpriteRenderer[] renderers;
        public PostProcessVolume ppVolume;
        public PostProcessProfile ambiantFlair, furyFlair;
        public ScreenHandler ScreenHandler;
        public Scoreboard Scoreboard;

        public float attackCooldown;
        public float attackDuration;
        public float verticalSpeed;
        
        public int Lane, killsForFury;
        public BoxCollider2D killzoneCollider;
        public CustomButton pressHandler;
        private float _verticalSlow = 1;
        public TextMeshProUGUI score;
        private float _formerScreenSize;

        private Vector3 _targetPos, _startPos;
        public float laneChangeSeconds;
        private float _laneTransitionLerpFactor = 1.5f;
        public bool fury;
        public float furyDuration;
        private float _furyTime;
        
        public float staminaMultiplier;

        private void Start()
        {
            _stamina = maxStamina;
            _changedStaminaMultiplier = staminaMultiplier;
            transform.position = 
                new Vector3(-ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + xPositioning, LaneManager.Manager.laneHeight * Lane);
        }

        private void Update()
        {
            GetTouchInput();
            GetKeyInput();
            AdjustPositionAccordingToScreenSize();
            UpdateStamina();
            UpdateFury();
            AdjustLanePosition();
            //animator.SetFloat("GameSpeed", Scroller.scroller.gameSpeed);
            if(Time.time >= _stunEnd) animator.SetBool("IsStunned", false);
        }

        void FixedUpdate()
        {
            EndAttacking();
            ResetAttackCooldown();
        }

        private void AdjustLaneLayer()
        {
            foreach (SpriteRenderer renderer in renderers) renderer.sortingLayerName = "Lane" + (Lane + 1);
        }

        private void AdjustLanePosition()
        {
            if (_laneTransitionLerpFactor <= 1)
            {
                transform.position = Vector3.Lerp(_startPos, _targetPos, _laneTransitionLerpFactor);
                _laneTransitionLerpFactor += Time.deltaTime /(laneChangeSeconds * _verticalSlow);
                if (_laneTransitionLerpFactor >= 0.95f)
                {
                    animator.SetBool("MovingUp", false);
                    animator.SetBool("MovingDown", false);
                }
            }
        }

        private void SetLaneTarget()
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            _startPos = pos;
            _targetPos = new Vector3(pos.x, Lane * LaneManager.Manager.laneHeight + LaneManager.Manager.spawnOffsetY, pos.z);
            _laneTransitionLerpFactor = 0;
        }

        private void UpdateFury()
        {
            if(!fury) furyBar.fillAmount = (float)_killsOfSameType / killsForFury;
            else
            {
                furyBar.fillAmount =  1 - (_furyTime);
                _furyTime += Time.deltaTime / furyDuration;
                if (_furyTime >= 1) EndFuryState();
            } 
        }

        

        public void StartSlow(float amount)
        {
            float percent = amount / 100;
            slowAmount = percent;
            _changedStaminaMultiplier = staminaMultiplier + percent;
            _verticalSlow = 1 + percent;
        }

        public void EndSlow()
        {
            slowAmount = 0.0f;
            _changedStaminaMultiplier = staminaMultiplier;
            _verticalSlow = 1;
        }
        
        private void EndAttacking()
        {
            if (AttackFinished())
            {
                _attacking = false;
            }
        }
        
        public void HandleKill()
        {
            if (_attacking)
            {
                GameObject other = killzone.InKillzone;
                if (other != null)
                {
                    IKillable toKill = other.GetComponent<IKillable>();
                    if (toKill != null && ((FGameObject)toKill).Lane == Lane && toKill.Kill(gameObject) && toKill is Animal)
                    {
                        AddStamina(((Animal) toKill).GetStamina());
                        ManageFury(((Animal) toKill).type);
                    }
                    _attacking = false;
                }
            }
        }

        private void ManageFury(EnemyType type)
        {
            if (type == _lastKillType)
            {
                _killsOfSameType++;
            }
            else
            {
                _killsOfSameType = 0;
                _lastKillType = type;
                _killsOfSameType++;
            }

            if (_killsOfSameType == killsForFury)
            {
                EnterFuryState();
            }
        }

        private void EnterFuryState()
        {
            fury = true;
            ScreenHandler.ShakeScreen(0.5f, new Vector2(0.1f,0.1f));
            EventHandler.BroadcastFuryEvent();
            ppVolume.profile = furyFlair;

        }
        private void EndFuryState()
        {
            fury = false;
            ScreenHandler.ShakeScreen(0.5f, new Vector2(0.1f,0.1f));
            ppVolume.profile = ambiantFlair;
            EventHandler.BroadcastSteakKill();
            _killsOfSameType = 0;
            _furyTime = 0;
        }

        private bool AttackFinished()
        {
            return _startAttack + attackDuration <= Time.time;
        }
        
        private void GetKeyInput()
        {
            if (!_attacking)
            {
                if (Input.GetKeyDown(KeyCode.W) || _topZone) MoveUp();
                if (Input.GetKeyDown(KeyCode.S) || _botZone) MoveDown();
            }

            if (Input.GetAxis("Jump") > 0) Attack();
        }
        
        private void ResetAttackCooldown()
        {
            if (Time.time >= _startAttack + attackCooldown) _hasAttacked = false;
        }
        
        public void ReduceStamina(float amount)
        {
            _stamina -= amount;
            _stamina = Mathf.Clamp(_stamina, 0, maxStamina);
        }
        
        private void GetTouchInput()
        {
            if (pressHandler.fingerPos.y <= (float) Screen.height / 2 && pressHandler.fingerPos.y > 0)
            {
                _botZone = true;
            }
            else if (pressHandler.fingerPos.y < 0)
            {
                _topZone = false;
                _botZone = false;
            }
            else if (pressHandler.fingerPos.y >= (float) Screen.height / 2 && pressHandler.fingerPos.y > 0)
            {
                _topZone = true;
            }
        }
        
        private void UpdateStamina()
        {
            if(!fury) ReduceStamina(_changedStaminaMultiplier * MovementHandler.Handler.GameSpeed * Time.deltaTime);
            staminaBar.fillAmount = _stamina / maxStamina;

            if (_stamina <= 0) Die();
        }
        
        private void AddStamina(float amount)
        {
            _stamina = Mathf.Clamp(_stamina + amount, 0, maxStamina);
        }
        
        private void AdjustPositionAccordingToScreenSize()
        {
            if (ScreenSizeChanged())
            {
                Transform trans = transform;
                trans.position =
                    new Vector3(
                        -ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + xPositioning,
                        trans.position.y);
            }
        }

        private bool ScreenSizeChanged()
        {
            if (ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) != _formerScreenSize)
            {
                _formerScreenSize = ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera);
                return true;
            }
            return false;
        }

        private void MoveUp()
        {
            /*Vector3 pos = transform.position;
            if (pos.y < stopperTop.position.y)
            {
                transform.position = pos + verticalSpeed * _verticalSlow * Time.deltaTime * Vector3.up;
                _movingUp = true;
            }
            else _movingUp = false;*/
            animator.SetBool("MovingUp", true);
            Lane++;
            Lane = Mathf.Clamp(Lane, 0, 3);
            SetLaneTarget();
            AdjustLaneLayer();
        }
        
        private void MoveDown()
        {
            /*Vector3 pos = transform.position;
            if (pos.y > stopperBottom.position.y)
            {
                transform.position = pos + verticalSpeed * _verticalSlow * Time.deltaTime * Vector3.down;
                _movingDown = true;
            }
            else _movingDown = false;*/
            
            Lane--;
            Lane = Mathf.Clamp(Lane, 0, 3);
            animator.SetBool("MovingDown", true);
            SetLaneTarget();
            AdjustLaneLayer();
        }
        
        public void Attack()
        {
            if (!_hasAttacked)
            {
                _hasAttacked = true;
                SetAttackStartTime();
                animator.SetTrigger("Attack");
                _attacking = true;
            }
        }

        private void SetAttackStartTime()
        {
            _startAttack = Time.time;
        }
        
        private void Die()
        {
            animator.SetTrigger("Death");
        }
        
        public void DisplayGameOverScreen()
        {
            score.text = ScoreHandler.Handler.GetScoreAsUnspacedScoreFormat();
            Scoreboard.Score = ScoreHandler.Handler.GetScoreAsUnspacedScoreFormat();
            Time.timeScale = 0;
            uiScreen.SetActive(false);
            gameOverScreen.SetActive(true);
        }
        
        public void ReduceGameSpeed()
        {
            MovementHandler.Handler.ReduceGameSpeed();
        }
        
        public void Stun(float time)
        {
            _stunEnd = Time.time + time;
            animator.SetBool("IsStunned", true);
        }
    }
}