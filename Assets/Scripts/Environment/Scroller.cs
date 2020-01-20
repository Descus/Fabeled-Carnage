using UnityEngine;
using Utility;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public float gameSpeed;

        public float slowAmount;

        public float speed = -1.5f;

        [Range(0, 300)] public int speedIncreaseInterval;
        
        private void Update()
        {
            gameSpeed = GetGameSpeed();
            if (Input.GetKeyDown(KeyCode.D))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Time.timeScale != 0.1f)
                    Time.timeScale = 0.1f;
                else
                    Time.timeScale = 1f;
            }

            EventHandler.BroadcastActorMove(speed * gameSpeed * Time.deltaTime);
            EventHandler.BroadcastBackgroundMove(speed * (1 - slowAmount) * gameSpeed * Time.deltaTime);
        }

        private float GetGameSpeed()
        {
            return 1 + (Mathf.Pow(1.01f, (int)Time.time) -1)* 0.2f;
        }

        
    }
}