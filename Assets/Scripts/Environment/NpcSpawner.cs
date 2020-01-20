using System;
using Actors;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Environment
{
    [ExecuteAlways]
    public class NpcSpawner : MonoBehaviour
    {
        private static int _enemiesOnField;
        public static float RightSreenX;
        private Camera _cam;
        private float _nextSpawn = 2;
        public float xPositioning;

        private float _timeStart;
        public ColorPrefab[] colorMappings;
        public Texture2D[] maps;
        public GameObject[,] onField;

        [Space(10)] [Range(0.0f, 50f)] public float spawnCooldownSec = 6f;

        private void Start()
        {
            _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            LaneManager.GenerateSpawns();

            _timeStart = Time.time;
        }

        private void Update()
        {
            AdjustSpawnPositions();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P)) OnSpawnPattern();

            if (_nextSpawn <= Time.time && _enemiesOnField == 0) OnSpawnPattern();
        }

        private void OnSpawnPattern()
        {
            _nextSpawn = Time.time + spawnCooldownSec;
            LaneManager.GenerateSpawns();
            GeneratePattern(GetNewMap());
            
            
        }

        private Texture2D GetNewMap()
        {
            return maps[Random.Range(0, maps.Length)];
        }

        private void GeneratePattern(Texture2D map)
        {

            for (int y = 0; y < map.height; y++)
            {
                for (int x = 0; x < map.width; x++)
                {
                    if (y > LaneManager.LANECOUNT) break;
                    GenerateTile(x, y, map);
                }

                float deviancy = GenerateDeviancy();
                EventHandler.OnDeviacySetEvent(deviancy,y);
            }
        }

        private void GenerateTile(int x, int y, Texture2D map)
        {
            Color pixelColor = map.GetPixel(x, y);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (pixelColor.a == 0) return;
            SpawnPrefab(x, y, pixelColor);
        }

        private void SpawnPrefab(int x, int y, Color color)
        {
            foreach (ColorPrefab colorMapping in colorMappings)
            {
                if (colorMapping.CompareColors(color))
                    if (y <= LaneManager.LANECOUNT && x <= LaneManager.SPAWNERCOUNT)
                    {
                        GameObject gObject = Instantiate(colorMapping.prefab, LaneManager.Spawns[y, x],
                            Quaternion.identity);
                        gObject.GetComponent<GameActor>().lane = y;
                        if (!gObject.CompareTag("Obstacle")) _enemiesOnField++;
                    }
            }
        }

        public static void RemoveEnemy()
        {
            _enemiesOnField--;
        }

        private void AdjustSpawnPositions()
        {
            RightSreenX = ScreenUtil.GetRightScreenBorderX(_cam);
            LaneManager.Spawnx = RightSreenX + 2;
        }

        private float GenerateDeviancy()
        {
            return (float) (.4f * Math.Tanh(Random.Range(-1.6f, 1.6f)));
        }
    }
}