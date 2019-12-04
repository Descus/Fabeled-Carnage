using Environment;
using Interfaces;
using UnityEngine;
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
        public int currentLane = 2;

        [Header("Killzone")] public BoxCollider2D killzoneCollider;

        private readonly bool snapToLane = false;

        [Header("Positioning")] public float speed = .1f;

        [ReadOnly] [SerializeField] private float stamina = maxStamina;

        [Header("Stamina")] public float staminaMult = 2.0f;

        public int xDefault = 4;

        private void Start()
        {
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = new Vector3(xDefault, LaneManager.LANEHEIGHT * 2);
            _killzone = killzoneCollider.GetComponent<Killzone>();
        }

        private void Update()
        {
            AdjustPos();
            HandleStamina();

            var pos = transform.position;

            if (!snapToLane)
            {
                if (pos.y < LaneManager.MINLANEY + LaneManager.LANEHEIGHT * (LaneManager.LANECOUNT - 1) &&
                    Input.GetAxis("Vertical") > 0) transform.position = pos + speed * Vector3.up;

                if (pos.y > LaneManager.MINLANEY && Input.GetAxis("Vertical") < 0)
                    transform.position = pos + speed * Vector3.down;
            }

            if (Input.GetAxis("Jump") > 0)
            {
                var other = _killzone.InKillzone;
                if (other != null)
                {
                    var toKill = other.GetComponent<ISKillable>();
                    if (toKill is Animal) AddStamina(((Animal) toKill).GetStamina());
                    toKill.Kill();
                }
            }

            //Snapping Enabled #Clunky AF
            //
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && snapToLane)
                if (currentLane < LaneManager.LANECOUNT - 1)
                    currentLane++;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && snapToLane)
                if (currentLane > 0)
                    currentLane--;

            if (snapToLane) transform.position = new Vector3(xDefault, LaneManager.Spawns[currentLane, 0].y, 0);

            //Debug Keybinds
            if (Input.GetKeyDown(KeyCode.Keypad8)) speed += 0.1f;
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
    }
}