using System;
using UnityEngine;

namespace Environment
{
    [ExecuteAlways]
    public class LaneManager : MonoBehaviour
    {
        public static float LANEHEIGHT = 1.75f;
        [Range(0.1f, 2.5f)]
        public float debuglaneheight = 1.75f;
        public static float COLWIDTH = 1.5f;
        public static readonly int LANECOUNT = 4;
        public static readonly float MINLANEY = 2.1f;
        public static float Spawnx = 11;
        public static readonly int SPAWNERCOUNT = 7;

        public static bool debug = true;
        //private static bool drawnOnce = false;

        public static readonly Vector3[,] Spawns = new Vector3[LANECOUNT, SPAWNERCOUNT];

        public static void GenerateSpawns()
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
                    Spawns[i, j] = new Vector3(Spawnx + COLWIDTH * j, i * debuglaneheight + MINLANEY, 0);
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(Spawns[i, j], 0.2f);
                }
        }

        private void OnDrawGizmosSelected()
        {
            Debug.Log(Spawnx);
        }
    }
}