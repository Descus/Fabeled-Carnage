using UnityEngine;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public delegate void MoveSubsriber(float speed);

        public float speed = 150f;

        public static event MoveSubsriber OnMoveUpdate;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Time.timeScale != 0.1f)
                    Time.timeScale = 0.1f;
                else
                    Time.timeScale = 1f;
            }

            if (OnMoveUpdate != null) OnMoveUpdate(speed / 100 * Time.deltaTime);
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