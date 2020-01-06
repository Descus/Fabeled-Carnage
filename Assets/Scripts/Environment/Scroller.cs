using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public delegate void MoveSubsriber(float speed);
        

        public float speed = -150f;
        public float slowAmount = 0;

        [ReadOnly]
        public float gameSpeed;
        [Range(0, 300)]
        public int speedIncreaseInterval;
        
        public static event MoveSubsriber OnActorMoveUpdate;
        public static event MoveSubsriber OnBackgroundMoveUpdate;

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

            if (OnActorMoveUpdate != null) OnActorMoveUpdate(speed / 100 * gameSpeed * (1 - slowAmount) * Time.deltaTime);
            if (OnBackgroundMoveUpdate != null) OnBackgroundMoveUpdate(speed / 100 * (1 - slowAmount) * gameSpeed * Time.deltaTime);
        }

        private float GetGameSpeed()
        {
            return  1 + (float)((int) Time.time / (speedIncreaseInterval/2)) / 2;
        }

        public static void SubscribeActorMoveEvent(MoveSubsriber add)
        {
            OnActorMoveUpdate += add;
        }

        public static void UnSubscribeActorMoveEvent(MoveSubsriber sub)
        {
            OnActorMoveUpdate -= sub;
        }
        
        public static void SubscribeBackgroundMoveEvent(MoveSubsriber add)
        {
            OnBackgroundMoveUpdate += add;
        }

        public static void UnSubscribeBackgroundMoveEvent(MoveSubsriber sub)
        {
            OnBackgroundMoveUpdate -= sub;
        }
    }
}