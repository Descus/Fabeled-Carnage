using Interfaces;
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
        
        
        
        public Killzone killzone;
        public Image staminaBar;
        public Animator animator;
        public Transform stopperTop, stopperBottom;
        public GameObject uiScreen, gameOverScreen;

        public float attackCooldown;
        public float attackDuration;
        public float verticalSpeed;
        
        public int Lane;
        public BoxCollider2D killzoneCollider;
        public CustomButton pressHandler;
        private float _verticalSlow = 1;

        public TextMeshProUGUI score;
        
        public float staminaMultiplier;

        private void Start()
        {
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
            ResetAttackCooldown();
            EndAttacking();
            //animator.SetFloat("GameSpeed", Scroller.scroller.gameSpeed);
            animator.SetBool("MovingUpDown", _movingUp || _movingDown);
            if(Time.time >= _stunEnd) animator.SetBool("IsStunned", false);
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
                    bool killed = toKill.Kill(gameObject);
                    if (killed && toKill is Animal)
                    {
                        AddStamina(((Animal) toKill).GetStamina());
                        AddScore(((Animal) toKill).GetScore());
                    }
                    _attacking = false;
                }
            }
        }
        
        private void AddScore(int score)
        {
            ScoreHandler.Handler.AddScore(score);
            ScoreHandler.Handler.ResetTimer();
            ScoreHandler.Handler.RegisterKill();
        }
        
        private bool AttackFinished()
        {
            return _startAttack + attackDuration <= Time.time;
        }
        
        private void GetKeyInput()
        {
            if (!_attacking)
            {
                if (Input.GetAxis("Vertical") > 0 || _topZone) MoveUp();
                else _movingUp = false;
                if (Input.GetAxis("Vertical") < 0 || _botZone) MoveDown();
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
            Transform trans = transform;
            trans.position = new Vector3(-ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + xPositioning, trans.position.y);
        }
        
        private void MoveUp()
        {
            Vector3 pos = transform.position;
            if (pos.y < stopperTop.position.y)
            {
                transform.position = pos + verticalSpeed * _verticalSlow * Time.deltaTime * Vector3.up;
                _movingUp = true;
            }
            else _movingUp = false;
                
        }
        
        private void MoveDown()
        {
            Vector3 pos = transform.position;
            if (pos.y > stopperBottom.position.y)
            {
                transform.position = pos + verticalSpeed * _verticalSlow * Time.deltaTime * Vector3.down;
                _movingDown = true;
            }
            else _movingDown = false;
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