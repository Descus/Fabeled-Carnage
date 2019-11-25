using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager: MonoBehaviour
{
    public static float LANEHEIGHT = 1f;
    public static float COLWIDTH = 1f;
    public static readonly int LANECOUNT = 5;
    public static readonly float MINLANEX = 0f;

    public static readonly Vector3[] LANES = new Vector3[LANECOUNT];

    public static void generateLanes()
    {
        for (int i = 0; i < LANECOUNT; i++)
        {
            LANES[i] = new Vector3(0, i * LANEHEIGHT + MINLANEX, 0);
        }
    }
}
