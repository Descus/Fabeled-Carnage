using System.Collections;
using Environment;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

namespace Actors.MainCharacter
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wolf : MonoBehaviour, IsSlowable
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


        [Header("Attack")] public float AttackCooldown;

        public float attackDuration;
        public int currentLane = 2;

        [Header("Debug")] public bool debug;

        public GameObject debugZone;

        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        [SerializeField] public CustomButton pressHandler;

        [Header("Movement")] public float speed = .1f;
        private float _vertSlow = 1;

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

        [Header("Positioning")] public int xDefault = 4;

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

        private void Start()
        {
            _changedStaminaMult = staminaMult;
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = new Vector3(xDefault, LaneManager.LANEHEIGHT * 2);
            _killzone = killzoneCollider.GetComponent<Killzone>();
            GameObject spawner = GameObject.Find("Spawner");
            _scroller = spawner.GetComponent<Scroller>();
            _npcSpawner = spawner.GetComponent<NpcSpawner>();
        }

        private void Update()
        {
            GetTouchInput();
            GetKeyInput();
            AdjustPos();
            HandleStamina();
            ResetAttackCooldown();
            HandleKill();
            EndAttacking();

            //TODO remove later
            HandleFixedLaning();
            HandleDebugKeybinds();
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

            ClampDebugValues();
        }

        private void ClampDebugValues()
        {
            speed = Mathf.Clamp(speed, 0, 1);
            _npcSpawner.spawnCooldownSec = Mathf.Clamp(_npcSpawner.spawnCooldownSec, 0.1f, 15);
            _scroller.speed = Mathf.Clamp(_scroller.speed, float.MinValue, 0);
        }

        private void HandleFixedLaning()
        {
//Snapping Enabled #Clunky AF
            //
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && _snapToLane)
                if (currentLane < LaneManager.LANECOUNT - 1)
                    currentLane++;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _snapToLane)
                if (currentLane > 0)
                    currentLane--;

            if (_snapToLane) transform.position = new Vector3(xDefault, LaneManager.Spawns[currentLane, 0].y, 0);
        }

        private void EndAttacking()
        {
            if (AttackFinished())
            {
                attacking = false;
                debugZone.SetActive(false);
            }
        }

        private void HandleKill()
        {
            if (attacking)
            {
                GameObject other = _killzone.InKillzone;
                if (other != null)
                {
                    ISKillable toKill = other.GetComponent<ISKillable>();
                    if (toKill is Animal) AddStamina(((Animal) toKill).GetStamina());
                    toKill.Kill();
                }
            }
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

                if (Input.GetAxis("Vertical") < 0 || _botZone) MoveDown();
            }

            if (Input.GetAxis("Jump") > 0) Attack();
        }

        private void ResetAttackCooldown()
        {
            if (Time.time >= startAttack + AttackCooldown) hasAttacked = false;
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
            stamina -= _changedStaminaMult * _scroller.gameSpeed * Time.deltaTime;
            _staminaBar.fillAmount = stamina / maxStamina;

            if (stamina <= 0) Time.timeScale = 0;
        }

        private void AddStamina(float amount)
        {
            stamina = Mathf.Clamp(stamina + amount, 0, maxStamina);
        }

        private void AdjustPos()
        {
            Transform transform1 = transform;
            transform1.position = new Vector3(-NpcSpawner.RightSreenX + xDefault, transform1.position.y);
        }

        private void MoveUp()
        {
            Vector3 pos = transform.position;
            if (pos.y < LaneManager.MINLANEY + LaneManager.LANEHEIGHT * (LaneManager.LANECOUNT - 1))
                transform.position = pos + speed * _vertSlow * Vector3.up;
        }

        private void MoveDown()
        {
            Vector3 pos = transform.position;
            if (pos.y > LaneManager.MINLANEY) transform.position = pos + speed * _vertSlow * Vector3.down;
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
            
        }
    }
}