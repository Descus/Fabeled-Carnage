using UnityEngine;
using Utility;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public delegate void MoveSubsriber(float speed);

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public float gameSpeed;

        public float slowAmount;


        public float speed = -1.5f;

        [Range(0, 300)] public int speedIncreaseInterval;

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

            if (OnActorMoveUpdate != null)
                OnActorMoveUpdate(speed * gameSpeed * (1 - slowAmount) * Time.deltaTime);
            if (OnBackgroundMoveUpdate != null)
                OnBackgroundMoveUpdate(speed * (1 - slowAmount) * gameSpeed * Time.deltaTime);
        }

        private float GetGameSpeed()
        {
            return 1 + (Mathf.Pow(1.01f, (int)Time.time) -1)* 0.2f;
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