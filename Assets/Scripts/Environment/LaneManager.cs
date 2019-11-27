using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager: MonoBehaviour
{
    public static float LANEHEIGHT = 1.5f;
    public static float COLWIDTH = 1.5f;
    public static readonly int LANECOUNT = 5;
    public static readonly float MINLANEY = 1.5f;
    public static float SPAWNX = 11;
    public static readonly int SPAWNERCOUNT = 6;

    public static readonly Vector3[,] SPAWNS = new Vector3[LANECOUNT, SPAWNERCOUNT];
    
    public static void generateSpawns()
    {
        for (int i = 0; i < LANECOUNT; i++)
        {
            for(int j = 0; j < SPAWNERCOUNT; j++)
            {
                SPAWNS[i, j] = new Vector3(SPAWNX + COLWIDTH * j, i * LANEHEIGHT + MINLANEY, 0);
                Instantiate(new GameObject("Spawn"), SPAWNS[i, j], Quaternion.identity);
            }
        }
    }
}
