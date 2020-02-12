using System;
using Rewrite.GameObjects;
using Rewrite.Handlers;
using Rewrite.Spawning;
using Unity.Mathematics;
using UnityEngine;
using EventHandler = Rewrite.Handlers.EventHandler;
using Random = UnityEngine.Random;

namespace Rewrite.Spawner
{
    public class NpcSpawner : MonoBehaviour
    {
        private int _enemiesOnField;
        public float initialSpawnCooldown;

        public float cooldownBetweenClearAndSpawn, spawnCooldownSeconds;
        private float _timeStart, _clearTimer, _nextSpawn;

        public ColorPrefab[] colorMappings;
        public Texture2D[] patternMapping;
        public GameObject steak;

        public static NpcSpawner Spawner;
        

        private void Start()
        {
            Spawner = this;
            _nextSpawn = initialSpawnCooldown;
            _timeStart = Time.time;
        }


        private void Update()
        {
            OnFieldEmpty();
            IncrementClearTimer();
        }

        private void FixedUpdate()
        {
            if (_nextSpawn <= Time.time && _clearTimer <= cooldownBetweenClearAndSpawn) SpawnPattern();
        }

        private void SpawnPattern()
        {
            SetNextSpawnTime();
            LaneManager.Manager.GenerateSpawns();
            GeneratePattern(GetNewMap());
        }

        private void SetNextSpawnTime()
        {
            _nextSpawn = Time.time + spawnCooldownSeconds;
        }

        private void GeneratePattern(Texture2D newMap)
        {
            for (int y = 0; y < newMap.height; y++)
            {
                for (int x = 0; x < newMap.width; x++)
                {
                    if (y > LaneManager.Manager.lanecount) break;
                    GenerateTile(x, y, newMap);
                }

                float deviancy = GenerateDeviancy();
                EventHandler.OnDeviacySetEvent(deviancy,y);
            }
        }

        private void GenerateTile(int x, int y, Texture2D newMap)
        {
            Color pixelColor = newMap.GetPixel(x, y);
            if(pixelColor.a == 0) return;
            if (!SceneObjectsHandler.Handler.playerObject.fury) SpawnPrefab(x, y, pixelColor);
            else
            {
                GameObject gObject = Instantiate(steak, LaneManager.Manager.Spawns[y, x], Quaternion.identity);
                gObject.GetComponent<FGameObject>().Lane = y;
            }
        }

        private void SpawnPrefab(int x, int y, Color pixelColor)
        {
            foreach (ColorPrefab colorMapping in colorMappings)
            {
                if(colorMapping.CompareColors(pixelColor))
                    if (y <= LaneManager.Manager.lanecount && x <= LaneManager.Manager.spawnerCount)
                    {
                        GameObject gObject = Instantiate(colorMapping.prefab, LaneManager.Manager.Spawns[y, x],
                            Quaternion.identity);
                        gObject.GetComponent<FGameObject>().Lane = y;
                    }
            }
        }

        public static void AddEnemyToField()
        {
            Spawner._enemiesOnField++;
        }
        
        public static void RemoveEnemyFromField()
        {
            Spawner._enemiesOnField--;
        }
        
        private Texture2D GetNewMap()
        {
            return patternMapping[Random.Range(0, patternMapping.Length)];
        }

        private void IncrementClearTimer()
        {
            _clearTimer += Time.deltaTime;
        }

        private void OnFieldEmpty()
        {
            if (_enemiesOnField == 0 || SceneObjectsHandler.Handler.playerObject.fury) ResetClearTimer();
        }

        private void ResetClearTimer()
        {
            _clearTimer = 0;
        }
        
        private float GenerateDeviancy()
        {
            return (float) (.4f * Math.Tanh(Random.Range(-1.6f, 1.6f)));
        }
    }
}