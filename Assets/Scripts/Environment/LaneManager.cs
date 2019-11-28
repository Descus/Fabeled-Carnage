using UnityEngine;

namespace Environment
{
    public class LaneManager: MonoBehaviour
    {
        public static float LANEHEIGHT = 1.5f;
        public static float COLWIDTH = 1.5f;
        public static readonly int LANECOUNT = 5;
        public static readonly float MINLANEY = 1.5f;
        public static float Spawnx = 11;
        public static readonly int SPAWNERCOUNT = 6;
        public static bool debug = false;

        public static readonly Vector3[,] Spawns = new Vector3[LANECOUNT, SPAWNERCOUNT];
    
        public static void GenerateSpawns()
        {
            for (int i = 0; i < LANECOUNT; i++)
            {
                for(int j = 0; j < SPAWNERCOUNT; j++)
                {
                    Spawns[i, j] = new Vector3(Spawnx + COLWIDTH * j, i * LANEHEIGHT + MINLANEY, 0);
                    if(debug) Instantiate(new GameObject("Spawn"), Spawns[i, j], Quaternion.identity);
                }
            }
        }
    }
}
