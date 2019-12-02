using Interfaces;
using UnityEngine;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public float speed = 10f;

        public delegate void MoveSubsriber(float speed);

        public static event MoveSubsriber OnMoveUpdate;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Time.timeScale != 0.1f)
                {
                    Time.timeScale = 0.1f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }

            if (OnMoveUpdate != null)
            {
                OnMoveUpdate(speed / 100);
            }
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
