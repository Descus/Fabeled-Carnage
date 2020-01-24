using System;
using UnityEngine;

namespace Environment
{
    [ExecuteAlways]
    public class LaneManager : MonoBehaviour
    {
        public  float LANEHEIGHT = 1.7f;
        public float COLWIDTH = 1.5f;
        public readonly int LANECOUNT = 4;
        public float MINLANEY = 2f;
        public float Spawnx = 11;
        public int SPAWNERCOUNT = 7;
        public static LaneManager manager;
        public Vector3[,] Spawns;
        [Range(0.1f, 3f)]
        public float debugMINLANEY;
        [Range(0.1f, 2.5f)]
        public float debuglaneheight = 1.75f;

        public Transform[] Despawner;
        
        public bool debug = true;
        //private static bool drawnOnce = false;

        void Start()
        {
            manager = this;
            Spawns =  new Vector3[LANECOUNT, SPAWNERCOUNT];
        }
        
        public void GenerateSpawns()
        {
            for (int i = 0; i < LANECOUNT; i++)
            for (int j = 0; j < SPAWNERCOUNT; j++)
                Spawns[i, j] = new Vector3(Spawnx + COLWIDTH * j, i * LANEHEIGHT + MINLANEY, 0);
        }

        private void OnDrawGizmos()
        {
            if (debug)
                for (int i = 0; i < LANECOUNT; i++)
                for (int j = 0; j < SPAWNERCOUNT; j++)
                {
                    Spawns =  new Vector3[LANECOUNT, SPAWNERCOUNT];
                    Spawns[i, j] = new Vector3(Spawnx + COLWIDTH * j, i * debuglaneheight + debugMINLANEY, 0);
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(Spawns[i, j], 0.2f);
                }
        }

        private void OnDrawGizmosSelected()
        {
            
        }
    }
}