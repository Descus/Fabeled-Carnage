using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Environment;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCSpawner : MonoBehaviour
{
    public Texture2D map;
    public ColorPrefab[] colorMappings;

    void Start()
    {
        LaneManager.generateSpawns();
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
        StartCoroutine(Wait(map));
    }

    private IEnumerator Wait(Texture2D map)
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                if (y > 5) break;
                GenerateTile(x, y, map);
            }
            yield return new WaitForSeconds(1);
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
                if (y <= LaneManager.LANECOUNT)
                {
                    int index = Random.Range(0, colorMapping.prefabs.Length);
                    Instantiate(colorMapping.prefabs[index].gameObject, LaneManager.SPAWNS[y], Quaternion.identity);
                }
            }
        }
    }
    
}
