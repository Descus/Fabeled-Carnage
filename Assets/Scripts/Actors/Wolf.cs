using Environment;
using UnityEngine;
using UnityEngine.UI;

namespace Actors
{
    public class Wolf: MonoBehaviour
    {
        public float speed = .1f;
        private bool snapToLane = false;
        public int currentLane = 2;
        public int xDefault = 4;
        public static float maxStamina = 100f;
        private float _stamina = maxStamina;
        public float staminaMult = 1.0f;
        private Image _staminaBar;

        void Start()
        {
            _staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            transform.position = new Vector3(xDefault, LaneManager.LANEHEIGHT * 2);
        }
    
        void Update()
        {
            AdjustPos();
            HandleStamina();
        
            Vector3 pos = transform.position;
        
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && !snapToLane)
            {
                if (pos.y < (LaneManager.MINLANEY + LaneManager.LANEHEIGHT * (LaneManager.LANECOUNT - 1)))
                {
                    transform.position = pos + Vector3.up * speed;
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && !snapToLane)
            {
                if (pos.y > LaneManager.MINLANEY)
                {
                    transform.position = pos + Vector3.down * speed;
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
            _stamina -= staminaMult * Time.deltaTime;
            _staminaBar.fillAmount = _stamina/maxStamina;
        }

        private void AdjustPos()
        {
            Transform transform1 = transform;
            transform1.position = new Vector3(-NpcSpawner.RightSreenX + xDefault, transform1.position.y);
        }
    }
}
