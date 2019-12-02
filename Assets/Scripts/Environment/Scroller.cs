using Interfaces;
using UnityEngine;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public float speed = 4;

        public delegate void MoveSubsriber(float speed);
        public static event MoveSubsriber onMoveUpdate;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Time.timeScale != 0.0f)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }

            if (onMoveUpdate != null)
            {
                onMoveUpdate(speed);
            }
        }
    }
}
