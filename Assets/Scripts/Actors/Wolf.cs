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
        private static readonly float maxStamina = 100f;
        private Killzone _killzone;
        private Image _staminaBar;

        [SerializeField] public CustomButton pressHandler;
        public Animator animator;
        public int currentLane = 2;
        private bool _topZone, _botZone, _attackZone, _snapToLane = false;

        public float AttackCooldown;
        private float startAttack;
        
        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        [Header("Positioning")] public float speed = .1f;

        [ReadOnly] [SerializeField] private float stamina = maxStamina;

        [Header("Stamina")] public float staminaMult = 2.0f;
        
        private bool _hasAttacked;

        public int xDefault = 4;

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
            
            if (!_snapToLane)
            {
                if (Input.GetAxis("Vertical") > 0 || _topZone) MoveUp();

                if (Input.GetAxis("Vertical") < 0 || _botZone) MoveDown();
            }

            if (Input.GetAxis("Jump") > 0)
            {
                if (!_hasAttacked)
                {
                    _hasAttacked = true;
                    Attack();
                }
                
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
            GameObject other = _killzone.InKillzone;
            if (other != null)
            {
                ISKillable toKill = other.GetComponent<ISKillable>();
                if (toKill is Animal) AddStamina(((Animal) toKill).GetStamina());
                toKill.Kill();
            }
        }

        public void OnButtonUpEnter()
        {
            _topZone = true;
        }
        public void OnButtonUpLeave()
        {
            _topZone = false;
        }

        public void OnButtonDownEnter()
        {
            _botZone = true;
        }
        public void OnButtonDownLeave()
        {
            _botZone = false;
        }

        public void OnButtonAttack()
        {
            Attack();
        }

        public void OnDrag()
        {
            Debug.Log("Drag");
        }
    }
}