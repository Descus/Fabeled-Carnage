using Enums;
using UnityEngine;
using Utility;

namespace Environment
{
    public class NpcSpawner : MonoBehaviour
    {
        private static int _enemiesOnField;
        private static readonly int MaxEnemies = 20;
        public static float RightSreenX;
        private Camera _cam;
        private float _nextSpawn = 6;

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

            if (_nextSpawn <= Time.time) OnSpawnPattern();
        }

        private void OnSpawnPattern()
        {
            
            if (_enemiesOnField < MaxEnemies)
            {
                _nextSpawn = Time.time + spawnCooldownSec;
                LaneManager.GenerateSpawns();
                if (_enemiesOnField == 0) GeneratePattern(GetNewMap());
            }
        }

        private Texture2D GetNewMap()
        {
            return maps[Random.Range(0, maps.Length)];
        }

        private void GeneratePattern(Texture2D map)
        {
            for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
            {
                if (y > 5) break;
                GenerateTile(x, y, map);
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
                if (colorMapping.color.Equals(color))
                    if (y <= LaneManager.LANECOUNT && x <= LaneManager.SPAWNERCOUNT)
                    {
                        //TODO Save Animal to array in order to move the whole lane at once on Touch
                        Instantiate(colorMapping.prefab, LaneManager.Spawns[y, x], Quaternion.identity);
                        _enemiesOnField++;
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
    }
}