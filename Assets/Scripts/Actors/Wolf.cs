using System;
using Environment;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

namespace Actors
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wolf : MonoBehaviour
    {
        [Header("Positioning")]
        public float speed = .1f;
        private bool snapToLane = false;
        public int currentLane = 2;
        public int xDefault = 4;

        [Header("Stamina")]
        public float staminaMult = 1.0f;
        private static float maxStamina = 100f;
        [ReadOnly][SerializeField]private float stamina = maxStamina;
        private Image _staminaBar;
        
        [Header("Killzone")]
        public BoxCollider2D killzoneCollider;
        private Killzone _killzone;

        void Start()
        {
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = new Vector3(xDefault, LaneManager.LANEHEIGHT * 2);
            _killzone = killzoneCollider.GetComponent<Killzone>();
        }

        void Update()
        {
            AdjustPos();
            HandleStamina();

            Vector3 pos = transform.position;

            if (!snapToLane)
            {
                if (pos.y < (LaneManager.MINLANEY + LaneManager.LANEHEIGHT * (LaneManager.LANECOUNT - 1)) && Input.GetAxis("Vertical") > 0)
                {
                    transform.position = pos + speed  * Vector3.up;
                }

                if (pos.y > LaneManager.MINLANEY && Input.GetAxis("Vertical") < 0)
                {
                    transform.position = pos + speed * Vector3.down;
                }
            }

            if (Input.GetAxis("Jump") > 0)
            {
                GameObject other = _killzone.InKillzone;
                if (other != null)
                {
                    ISKillable toKill = other.GetComponent<ISKillable>();
                    if (toKill is Animal)
                    {
                        AddStamina(((Animal)toKill).GetStamina());
                    }
                    toKill.Kill();
                }
                
            }

            //Snapping Enabled #Clunky AF
            //
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && snapToLane)
            {
                if (currentLane < LaneManager.LANECOUNT - 1)
                {
                    currentLane++;
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && snapToLane)
            {
                if (currentLane > 0)
                {
                    currentLane--;
                }
            }

            if (snapToLane)
            {
                transform.position = new Vector3(xDefault, LaneManager.Spawns[currentLane, 0].y, 0);
            }

            //Debug Keybinds
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                speed += 0.1f;
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
            Transform transform1 = transform;
            transform1.position = new Vector3(-NpcSpawner.RightSreenX + xDefault, transform1.position.y);
        }
    }
}
