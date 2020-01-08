using Environment;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Actors.MainCharacter
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wolf : MonoBehaviour, IsSlowable
    {
        
        private Killzone _killzone;
        private Image _staminaBar;

        [SerializeField] public CustomButton pressHandler;
        public Animator animator;
        

        [Header("Attack")]
        public float AttackCooldown;
        public float attackDuration;
#if UNITY_EDITOR 
        [ReadOnly]
#endif 
        [SerializeField] private float startAttack;
#if UNITY_EDITOR 
        [ReadOnly] 
#endif 
        [SerializeField] private bool _hasAttacked;
#if UNITY_EDITOR 
        [ReadOnly] 
#endif 
        [SerializeField] private bool _attacking = false;
        
        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        [Header("Movement")] public float speed = .1f;
        
        [Header("Stamina")] public float staminaMult = 2.0f;
        private float _changedStaminaMult;
        private static readonly float maxStamina = 100f;
        
#if UNITY_EDITOR 
        [ReadOnly] 
#endif 
        [SerializeField] private float stamina = maxStamina;

        private Scroller _scroller;
        private NpcSpawner _npcSpawner;
        
        [Header("Debug")]
        public bool debug;
        public GameObject debugZone;

        [Header("Positioning")]
        public int xDefault = 4;
        public int currentLane = 2;
        private bool _topZone, _botZone, _attackZone, _snapToLane = false;
        
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
            AdjustPos();
            HandleStamina();
            ResetAttackCooldown();
            
            if (!_snapToLane && !_attacking)
            {
                if (Input.GetAxis("Vertical") > 0 || _topZone) MoveUp();

                if (Input.GetAxis("Vertical") < 0 || _botZone) MoveDown();
            }

            if (Input.GetAxis("Jump") > 0)
            {
                
                Attack();
            }

            
            if (_attacking)
            {
                GameObject other = _killzone.InKillzone;
                if (other != null)
                {
                    ISKillable toKill = other.GetComponent<ISKillable>();
                    if (toKill is Animal) AddStamina(((Animal) toKill).GetStamina());
                    toKill.Kill();
                }
            }

            if (startAttack + attackDuration <= Time.time)
            {
                _attacking = false;
                debugZone.SetActive(false);
            }
            //Snapping Enabled #Clunky AF
            //
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && _snapToLane)
                if (currentLane < LaneManager.LANECOUNT - 1)
                    currentLane++;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _snapToLane)
                if (currentLane > 0)
                    currentLane--;

            if (_snapToLane) transform.position = new Vector3(xDefault, LaneManager.Spawns[currentLane, 0].y, 0);

            //Debug Keybinds
            if (Input.GetKeyDown(KeyCode.Keypad8)&&!Input.GetKey(KeyCode.LeftShift)) speed += 0.1f;
            if (Input.GetKeyDown(KeyCode.Keypad2)&&!Input.GetKey(KeyCode.LeftShift)) speed -= 0.1f;
            if (Input.GetKeyDown(KeyCode.Keypad8)&&Input.GetKey(KeyCode.LeftShift)) speed += 0.01f;
            if (Input.GetKeyDown(KeyCode.Keypad2)&&Input.GetKey(KeyCode.LeftShift)) speed -= 0.01f;


            if (Input.GetKeyDown(KeyCode.KeypadPlus)&&!Input.GetKey(KeyCode.LeftShift)) _npcSpawner.spawnCooldownSec += 1;
            if (Input.GetKeyDown(KeyCode.KeypadMinus)&&!Input.GetKey(KeyCode.LeftShift)) _npcSpawner.spawnCooldownSec -= 1;
            if (Input.GetKeyDown(KeyCode.KeypadPlus)&&Input.GetKey(KeyCode.LeftShift)) _npcSpawner.spawnCooldownSec += 0.1f;
            if (Input.GetKeyDown(KeyCode.KeypadMinus)&&Input.GetKey(KeyCode.LeftShift)) _npcSpawner.spawnCooldownSec -= 0.1f;
            
            if (Input.GetKeyDown(KeyCode.Keypad6)&&!Input.GetKey(KeyCode.LeftShift)) _scroller.speed += 10;
            if (Input.GetKeyDown(KeyCode.Keypad4)&&!Input.GetKey(KeyCode.LeftShift)) _scroller.speed -= 10;
            if (Input.GetKeyDown(KeyCode.Keypad6)&&Input.GetKey(KeyCode.LeftShift)) _scroller.speed += 1;
            if (Input.GetKeyDown(KeyCode.Keypad4)&&Input.GetKey(KeyCode.LeftShift)) _scroller.speed -= 1;

            speed = Mathf.Clamp(speed, 0, 1);
            _npcSpawner.spawnCooldownSec = Mathf.Clamp(_npcSpawner.spawnCooldownSec, 0.1f, 15);
            _scroller.speed = Mathf.Clamp(_scroller.speed, float.MinValue ,-10);
        }

        private void ResetAttackCooldown()
        {
            if (Time.time >= startAttack + AttackCooldown)
            {
                _hasAttacked = false;
            }
        }

        private void GetTouchInput()
        {
            if (pressHandler.fingerPos.y <= (float)Screen.height / 2 && pressHandler.fingerPos.y > 0)
            {
                _botZone = true;
            }
            else if (pressHandler.fingerPos.y < 0)
            {
                _topZone = false;
                _botZone = false;
            } 
            else if((pressHandler.fingerPos.y >= (float)Screen.height / 2 && pressHandler.fingerPos.y > 0))
            {
                _topZone = true;
            }
        }

        private void HandleStamina()
        {
            stamina -= _changedStaminaMult * _scroller.gameSpeed * Time.deltaTime;
            _staminaBar.fillAmount = stamina / maxStamina;

            if (stamina <= 0)
            {
                Time.timeScale = 0;
            }
        }

        private void AddStamina(float amount)
        {
            stamina = Mathf.Clamp(stamina + amount, 0, maxStamina);
        }

        private void AdjustPos()
        {
            var transform1 = transform;
            transform1.position = new Vector3(-NpcSpawner.RightSreenX + xDefault, transform1.position.y);
        }

        void MoveUp()
        {
            Vector3 pos = transform.position;
            if (pos.y < LaneManager.MINLANEY + LaneManager.LANEHEIGHT * (LaneManager.LANECOUNT - 1))
            {
                transform.position = pos + speed * Vector3.up;
            }
        }
        void MoveDown()
        {
            Vector3 pos = transform.position;
            if (pos.y > LaneManager.MINLANEY)
            {
                transform.position = pos + speed * Vector3.down; 
            }
        }

        public void Attack()
        {
            if (!_hasAttacked)
            {
                _hasAttacked = true;
                startAttack = Time.time;
                animator.SetTrigger("Attack");
                _attacking = true;
                if (debug)
                {
                    debugZone.SetActive(true);
                } 
            }
        }
        
        public void StartSlow(float amount)
        {
            _scroller.slowAmount = amount / 100;
            _changedStaminaMult *= 1 + (amount / 100);
        }

        public void EndSlow()
        {
            _scroller.slowAmount = 0.0f;
            _changedStaminaMult = staminaMult;
        }
    }
}