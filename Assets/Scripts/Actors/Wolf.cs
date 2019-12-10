using Environment;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

namespace Actors
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wolf : MonoBehaviour
    {
        
        private Killzone _killzone;
        private Image _staminaBar;

        [SerializeField] public CustomButton pressHandler;
        public Animator animator;
        

        [Header("Attack")]
        public float AttackCooldown;
        public float attackDuration;
        [ReadOnly] [SerializeField] private float startAttack;
        [ReadOnly] [SerializeField] private bool _hasAttacked;
        [ReadOnly] [SerializeField] private bool _attacking = false;
        
        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        [Header("Movement")] public float speed = .1f;
        
        [Header("Stamina")] public float staminaMult = 2.0f;
        private static readonly float maxStamina = 100f;
        [ReadOnly] [SerializeField] private float stamina = maxStamina;
        
        
        [Header("Debug")]
        public bool debug;
        public GameObject debugZone;

        [Header("Positioning")]
        public int xDefault = 4;
        public int currentLane = 2;
        private bool _topZone, _botZone, _attackZone, _snapToLane = false;
        
        private void Start()
        {
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = new Vector3(xDefault, LaneManager.LANEHEIGHT * 2);
            _killzone = killzoneCollider.GetComponent<Killzone>();
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

            if (Input.GetAxis("Jump") > 0 && !_hasAttacked)
            {
                _hasAttacked = true;
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
            if (Input.GetKeyDown(KeyCode.Keypad8)) speed += 0.1f;
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
            stamina -= staminaMult * Time.deltaTime;
            _staminaBar.fillAmount = stamina / maxStamina;
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

        void Attack()
        {
            startAttack = Time.time;
            animator.SetTrigger("Attack");
            _attacking = true;
            if (debug)
            {
                debugZone.SetActive(true);
            }
        }
    }
}