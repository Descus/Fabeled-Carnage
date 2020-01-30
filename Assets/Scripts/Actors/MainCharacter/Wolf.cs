using System.Collections;
using Environment;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

namespace Actors.MainCharacter
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wolf : MonoBehaviour, ISlowable
    {
        private static readonly float maxStamina = 100f;
        private readonly bool _snapToLane = false;
#if UNITY_EDITOR
        [FormerlySerializedAs("_attacking")]
        [ReadOnly]
#endif
        [SerializeField]
        private bool attacking;

        private float _changedStaminaMult;
#if UNITY_EDITOR
        [FormerlySerializedAs("_hasAttacked")]
        [ReadOnly]
#endif
        [SerializeField]
        private bool hasAttacked;

        private Killzone _killzone;
        private NpcSpawner _npcSpawner;

        private Scroller _scroller;
        private Image _staminaBar;
        private bool _topZone, _botZone, _attackZone;
        public Animator animator;
        public Transform stopperTop, stopperBottom;
        private bool movingUp, movingDown;
        public GameObject uiScreen, gameOverScreen;

        [Header("Attack")] public float attackCooldown;

        public float attackDuration;
        public int currentLane = 2;

        [Header("Debug")] public bool debug;

        public GameObject debugZone;

        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        [SerializeField] public CustomButton pressHandler;

        [Header("Movement")] public float speed = .1f;
        private float _vertSlow = 1;
        public TextMeshProUGUI score;
        private float stunEnd;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField]
        private float stamina = maxStamina;

        [Header("Stamina")] public float staminaMult = 2.0f;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField]
        private float startAttack;
        
        private void Start()
        {
            _changedStaminaMult = staminaMult;
            GameObject spawner = GameObject.Find("Spawner");
            _scroller = spawner.GetComponent<Scroller>();
            _npcSpawner = spawner.GetComponent<NpcSpawner>();
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = 
                new Vector3(-NpcSpawner.RightSreenX + _npcSpawner.xPositioning, LaneManager.manager.LANEHEIGHT * 2);
            _killzone = killzoneCollider.GetComponent<Killzone>();
        }

        private void Update()
        {
            GetTouchInput();
            GetKeyInput();
            AdjustPos();
            HandleStamina();
            ResetAttackCooldown();
            EndAttacking();
            //animator.SetFloat("GameSpeed", Scroller.scroller.gameSpeed);
            animator.SetBool("MovingUpDown", movingUp || movingDown);
            //TODO remove later
            HandleFixedLaning();
            HandleDebugKeybinds();
            
            if(Time.time >= stunEnd) animator.SetBool("IsStunned", false);
        }

        private void HandleDebugKeybinds()
        {
//Debug Keybinds
            if (Input.GetKeyDown(KeyCode.Keypad8) && !Input.GetKey(KeyCode.LeftShift)) speed += 0.1f;
            if (Input.GetKeyDown(KeyCode.Keypad2) && !Input.GetKey(KeyCode.LeftShift)) speed -= 0.1f;
            if (Input.GetKeyDown(KeyCode.Keypad8) && Input.GetKey(KeyCode.LeftShift)) speed += 0.01f;
            if (Input.GetKeyDown(KeyCode.Keypad2) && Input.GetKey(KeyCode.LeftShift)) speed -= 0.01f;


            if (Input.GetKeyDown(KeyCode.KeypadPlus) && !Input.GetKey(KeyCode.LeftShift))
                _npcSpawner.spawnCooldownSec += 1;
            if (Input.GetKeyDown(KeyCode.KeypadMinus) && !Input.GetKey(KeyCode.LeftShift))
                _npcSpawner.spawnCooldownSec -= 1;
            if (Input.GetKeyDown(KeyCode.KeypadPlus) && Input.GetKey(KeyCode.LeftShift))
                _npcSpawner.spawnCooldownSec += 0.1f;
            if (Input.GetKeyDown(KeyCode.KeypadMinus) && Input.GetKey(KeyCode.LeftShift))
                _npcSpawner.spawnCooldownSec -= 0.1f;

            if (Input.GetKeyDown(KeyCode.Keypad6) && !Input.GetKey(KeyCode.LeftShift)) _scroller.speed += 10;
            if (Input.GetKeyDown(KeyCode.Keypad4) && !Input.GetKey(KeyCode.LeftShift)) _scroller.speed -= 10;
            if (Input.GetKeyDown(KeyCode.Keypad6) && Input.GetKey(KeyCode.LeftShift)) _scroller.speed += 1;
            if (Input.GetKeyDown(KeyCode.Keypad4) && Input.GetKey(KeyCode.LeftShift)) _scroller.speed -= 1;
        }

        

        private void HandleFixedLaning()
        {
//Snapping Enabled #Clunky AF
            //
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && _snapToLane)
                if (currentLane < LaneManager.manager.LANECOUNT - 1)
                    currentLane++;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _snapToLane)
                if (currentLane > 0)
                    currentLane--;

            if (_snapToLane) transform.position = new Vector3(_npcSpawner.xPositioning, LaneManager.manager.Spawns[currentLane, 0].y, 0);
        }

        private void EndAttacking()
        {
            if (AttackFinished())
            {
                attacking = false;
                debugZone.SetActive(false);
            }
        }

        public void HandleKill()
        {
            if (attacking)
            {
                GameObject other = _killzone.InKillzone;
                if (other != null)
                {
                    IKillable toKill = other.GetComponent<IKillable>();
                    bool killed = toKill.Kill(gameObject);
                    if (killed && toKill is Animal)
                    {
                        AddStamina(((Animal) toKill).GetStamina());
                        AddScore(((Animal) toKill).GetScore());
                    }
                    attacking = false;
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
            return startAttack + attackDuration <= Time.time;
                 }

        private void GetKeyInput()
        {
            if (!_snapToLane && !attacking)
            {
                if (Input.GetAxis("Vertical") > 0 || _topZone) MoveUp();
                else movingUp = false;
                if (Input.GetAxis("Vertical") < 0 || _botZone) MoveDown();
                else movingDown = false;
            }

            if (Input.GetAxis("Jump") > 0) Attack();
        }

        private void ResetAttackCooldown()
        {
            if (Time.time >= startAttack + attackCooldown) hasAttacked = false;
        }

        public void ReduceStamina(float amount)
        {
            stamina -= amount;
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

        private void HandleStamina()
        {
            ReduceStamina(_changedStaminaMult * _scroller.gameSpeed * Time.deltaTime);
            _staminaBar.fillAmount = stamina / maxStamina;

            if (stamina <= 0) Die();
        }

        private void AddStamina(float amount)
        {
            stamina = Mathf.Clamp(stamina + amount, 0, maxStamina);
        }

        private void AdjustPos()
        {
            var transform1 = transform;
            transform1.position = new Vector3(-NpcSpawner.RightSreenX + _npcSpawner.xPositioning, transform1.position.y);
        }

        private void MoveUp()
        {
            Vector3 pos = transform.position;
            if (pos.y < stopperTop.position.y)
            {
                transform.position = pos + speed * _vertSlow * Time.deltaTime * Vector3.up;
                movingUp = true;
            }
            else movingUp = false;
                
        }

        private void MoveDown()
        {
            Vector3 pos = transform.position;
            if (pos.y > stopperBottom.position.y)
            {
                transform.position = pos + speed * _vertSlow * Time.deltaTime * Vector3.down;
                movingDown = true;
            }
            else movingDown = false;


        }

        public void Attack()
        {
            if (!hasAttacked)
            {
                hasAttacked = true;
                startAttack = Time.time;
                animator.SetTrigger("Attack");
                attacking = true;
                if (debug) debugZone.SetActive(true);
            }
        }

        public void Die()
        {
            // animator.SetTrigger("Death");
        }
        public void StartSlow(float amount)
        {
            float percent = amount / 100;
            _scroller.slowAmount = percent;
            _changedStaminaMult = staminaMult + percent;
            _vertSlow = 1 - percent;
        }

        public void EndSlow()
        {
            _scroller.slowAmount = 0.0f;
            _changedStaminaMult = staminaMult;
            _vertSlow = 1;
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
            Scroller.scroller.ReduceGameSpeed();
        }

        public void Stun(float time)
        {
            stunEnd = Time.time + time;
            animator.SetBool("IsStunned", true);
        }
    }
}