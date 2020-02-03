using System;
using Rewrite.Handlers;
using Rewrite.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rewrite
{
    public class LaneManager: MonoBehaviour
    {
        public static LaneManager Manager;

        private float _despawnXPos;
        public float spawnOffsetX, spawnOffsetY, columnWidth, laneHeight;
        
        public Vector3[,] Spawns;

        public int lanecount, spawnerCount;
        
        public bool debug;

        public Transform[] particleDespawnerPlane;

        public float DespawnXPos => _despawnXPos;

        private void Start()
        {
            Manager = this;
            Spawns = new Vector3[lanecount, spawnerCount];
            
            AdjustSpawnPositions();
            GenerateSpawns();
        }

        private void Update()
        {
            AdjustSpawnPositions();
        }

        private void AdjustSpawnPositions()
        {
            spawnOffsetX = ScreenUtil.GetRightScreenBorderX(SceneObjectsHandler.Handler.mainCamera) + 2;
            _despawnXPos = -spawnOffsetX;
        }

        public void GenerateSpawns()
        {
            for (int i = 0; i < lanecount; i++)
            for (int j = 0; j < spawnerCount; j++)
                Spawns[i, j] = new Vector3(spawnOffsetX + columnWidth * j, i * laneHeight + spawnOffsetY, 0);
        }
        
        private void OnDrawGizmos()
        {
            if (debug)
                for (int i = 0; i < lanecount; i++)
                for (int j = 0; j < spawnerCount; j++)
                {
                    Spawns =  new Vector3[lanecount, spawnerCount];
                    Spawns[i, j] = new Vector3(spawnOffsetX + columnWidth * j, i * laneHeight + spawnOffsetY, 0);
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(Spawns[i, j], 0.2f);
                }
        }
    }
}