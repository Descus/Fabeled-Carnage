using System;
using Interfaces;
using Rewrite.Enums;
using Rewrite.GameObjects.Actors;
using Rewrite.Handlers;
using Rewrite.UI;
using Rewrite.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewrite.GameObjects.MainCharacter
{
    public class Wolf: MonoBehaviour, ISlowable
    {
        public float slowAmount;
        public float speed;
        public float maxStamina;
        public float xPositioning;

        private bool _attacking, _hasAttacked, _topZone, _botZone, _attackZone, _movingUp, _movingDown;
        private float _changedStaminaMultiplier, _stunEnd, _stamina, _startAttack;
        private int _killsOfSameType;
        private EnemyType _lastKillType;
        
        
        
        public Killzone killzone;
        public Image staminaBar, furyBar;
        public Animator animator;
        public Transform stopperTop, stopperBottom;
        public GameObject uiScreen, gameOverScreen;
        public SpriteRenderer[] renderers;

        public float attackCooldown;
        public float attackDuration;
        public float verticalSpeed;
        
        public int Lane, killsForFury;
        public BoxCollider2D killzoneCollider;
        public CustomButton pressHandler;
        private float _verticalSlow = 1;
        public TextMeshProUGUI score;
        private float formerScreenSize;
        
        public float staminaMultiplier;
        

        private void Start()
        {
            _stamina = maxStamina;
            _changedStaminaMultiplier = staminaMultiplier;
            transform.position = 
                new Vector3(-ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + xPositioning, LaneManager.Manager.laneHeight * 2);
        }

        private void Update()
        {
            GetTouchInput();
            GetKeyInput();
            AdjustPositionAccordingToScreenSize();
            UpdateStamina();
            UpdateFury();
            //animator.SetFloat("GameSpeed", Scroller.scroller.gameSpeed);
            animator.SetBool("MovingUpDown", _movingUp || _movingDown);
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

        private void AdjustLanePoistion()
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            pos = new Vector3(pos.x, Lane * LaneManager.Manager.laneHeight + LaneManager.Manager.spawnOffsetY, pos.z);
            trans.position = pos;
        }

        private void UpdateFury()
        {
            furyBar.fillAmount = (float)_killsOfSameType / killsForFury;
        }

        public void StartSlow(float amount)
        {
            float percent = amount / 100;
            slowAmount = percent;
            _changedStaminaMultiplier = staminaMultiplier + percent;
            _verticalSlow = 1 - percent;
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
                    if (((FGameObject)toKill).Lane == Lane && toKill.Kill(gameObject) && toKill is Animal)
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
                else _movingUp = false;
                if (Input.GetKeyDown(KeyCode.S) || _botZone) MoveDown();
                else _movingDown = false;
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
            ReduceStamina(_changedStaminaMultiplier * MovementHandler.GameSpeed * Time.deltaTime);
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
            if (ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) != formerScreenSize)
            {
                formerScreenSize = ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera);
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
            Lane++;
            Lane = Mathf.Clamp(Lane, 0, 3);
            AdjustLanePoistion();
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
            AdjustLanePoistion();
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
            // animator.SetTrigger("Death");
        }
        
        public void DisplayGameOverScreen()
        {
            score.text = ScoreHandler.Handler.ConvertToScoreFormat(ScoreHandler.Handler.score);
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