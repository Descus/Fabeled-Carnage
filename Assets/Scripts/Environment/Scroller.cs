using Interfaces;
using UnityEngine;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        public float speed = 4;
    
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

            GameObject[] movables = GameObject.FindGameObjectsWithTag("Movable");
            foreach (GameObject movable in movables)
            {
                ISMovable mov = movable.GetComponent<ISMovable>();
                mov.Move(speed * Time.deltaTime);
                if (movable.transform.position.x <= -LaneManager.Spawnx)
                {
                    Destroy(movable);
                    NpcSpawner.ReduceEnemyCount();
                }
            }
        }
    }
}
