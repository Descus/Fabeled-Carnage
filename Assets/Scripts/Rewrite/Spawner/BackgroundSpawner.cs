using Rewrite.Handlers;
using Rewrite.Utility;
using UnityEngine;


namespace Rewrite.Spawner
{
    public class BackgroundSpawner: MonoBehaviour
    {
        public static BackgroundSpawner Spawner;
        private float _despawnXPos;

        public GameObject[] backgrounds;
        public float tileWidth = 13;
        
        private int _backgroundsSpawned = 0;

        private Camera _cam;
        private GameObject _latest;

        public float DespawnXPos
        {
            get => _despawnXPos;
        }

        private void Start()
        {
            Spawner = this;
            _cam = SceneObjectsHandler.Handler.mainCamera;
            SetDespawnXPosition();
            SpawnBackground(-tileWidth);
            SpawnBackground(0);
            SpawnBackground(tileWidth);
        }
        
        private void Update()
        {
            if (_latest.transform.position.x <= tileWidth) SpawnBackground(_latest.transform.position.x + tileWidth);
            SetDespawnXPosition();
        }

        private void SpawnBackground(float positionX)
        {
            _latest = Instantiate(GetNextTile(), new Vector3(positionX, 5), Quaternion.identity);
            if (_backgroundsSpawned + 1 < backgrounds.Length) _backgroundsSpawned++;
        }

        private GameObject GetNextTile()
        {
            return backgrounds[_backgroundsSpawned];
        }

        private void SetDespawnXPosition()
        {
            _despawnXPos = -ScreenUtil.GetRightScreenBorderX(_cam) - 1;
        }
    }
}