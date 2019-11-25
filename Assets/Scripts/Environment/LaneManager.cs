using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager: MonoBehaviour
{
    public static float LANEHEIGHT = 1.5f;
    public static float COLWIDTH = 1f;
    public static readonly int LANECOUNT = 5;
    public static readonly float MINLANEY = 0f;
    public static readonly int SPAWNX = 11;

    public static readonly Vector3[] SPAWNS = new Vector3[LANECOUNT];

    public static void generateSpawns()
    {
        for (int i = 0; i < LANECOUNT; i++)
        {
            SPAWNS[i] = new Vector3(SPAWNX, i * LANEHEIGHT + MINLANEY, 0);
        }
    }
}
