using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public delegate void MoveSubsriber(float speed);

        public float speed = 150f;

        [ReadOnly]
        public float gameSpeed;
        [Range(0, 300)]
        public int speedIncreaseInterval;
        
        public static event MoveSubsriber OnMoveUpdate;

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

            if (OnMoveUpdate != null) OnMoveUpdate(speed / 100 * gameSpeed * Time.deltaTime);
        }

        private float GetGameSpeed()
        {
            return  1 + (float)((int) Time.time / (speedIncreaseInterval/2)) / 2;
        }

        public static void SubscribeMoveEvent(MoveSubsriber add)
        {
            OnMoveUpdate += add;
        }

        public static void UnSubscribeMoveEvent(MoveSubsriber sub)
        {
            OnMoveUpdate -= sub;
        }
    }
}