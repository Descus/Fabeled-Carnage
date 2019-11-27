using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCSpawner : MonoBehaviour
{
    public static int EnemiesOnField = 0;
    public static int MaxEnemies = 20;
    public float spawnCooldownSec = 2f;
    public Texture2D[] maps;
    public ColorPrefab[] colorMappings;
    private float _nextSpawn = 2;
    public static float rightSreenX;
    private Camera _cam;
    
    void Start()
    { 
        _cam= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        LaneManager.generateSpawns();
    }

    void Update()
    {
        adjustSpawnPositions();
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnSpawnPattern();
        }

        if (_nextSpawn <= Time.time)
        {
            OnSpawnPattern();
        }
    }

    void OnSpawnPattern()
    {
        _nextSpawn = Time.time + spawnCooldownSec;
        if (EnemiesOnField < MaxEnemies)
        {
            LaneManager.generateSpawns();
            GeneratePattern(GetNewMap());
        }
    }

    Texture2D GetNewMap()
    {
        return maps[Random.Range(0, maps.Length)];
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
                if (y <= LaneManager.LANECOUNT && x <= LaneManager.SPAWNERCOUNT)
                {
                    int index = Random.Range(0, colorMapping.prefabs.Length);
                    Instantiate(colorMapping.prefabs[index].gameObject, LaneManager.SPAWNS[y, x], Quaternion.identity);
                    EnemiesOnField++;
                }
            }
        }
    }

    public static void ReduceEnemyCount()
    {
        EnemiesOnField--;
    }

    private void adjustSpawnPositions()
    {
        rightSreenX = ScreenUtil.getRightScreenBorderX(_cam);
        LaneManager.SPAWNX = rightSreenX + 2;
    }
}