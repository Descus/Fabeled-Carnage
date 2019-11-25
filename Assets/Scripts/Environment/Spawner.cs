using System;
using System.Collections;
using System.Collections.Generic;
using Environment;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Texture2D map;
    public ColorPrefab[] colorMappings;
    private System.Random rnd = new System.Random();

    void Start()
    {
        LaneManager.generateLanes();
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnSpawnPattern();
        }
    }

    void OnSpawnPattern()
    {
        GeneratePattern(GetNewMap());
    }

    Texture2D GetNewMap()
    {
        return map;
    }

    void GeneratePattern(Texture2D map)
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                if (y > 5) break;
                GenerateTile(x, y, map);
            }
        }
    }

    void GenerateTile(int x, int y, Texture2D map)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            return;
        }
        SpawnPrefab(x, y, pixelColor);
    }

    private void SpawnPrefab(int x, int y, Color color)
    {
        foreach (ColorPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(color))
            {
                int index = rnd.Next(colorMapping.prefabs.Length - 1);
                Instantiate(colorMapping.prefabs[index].gameObject, new Vector3(LaneManager.COLWIDTH * x, LaneManager.LANEHEIGHT * y), Quaternion.identity);
            }
        }
    }
    
}
