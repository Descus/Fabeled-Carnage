using Structs;
using UnityEngine;
using Utility;

namespace Environment
{
    public class BackgroundSpawner : MonoBehaviour
    {
        public static float SpawnX;
        public BackgroundTile[] backgroundTiles;

        private Camera _cam;
        private GameObject _latest;

        public float tileWidth = 16;

        // Start is called before the first frame update
        private void Start()
        {
            _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            SpawnX = ScreenUtil.GetRightScreenBorderX(_cam) + 1;
            SpawnBackground(-tileWidth);
            SpawnBackground(0);
            SpawnBackground(tileWidth);
        }

        private void Update()
        {
            SpawnX = ScreenUtil.GetRightScreenBorderX(_cam) + 1;
            if (_latest.transform.position.x <= tileWidth) SpawnBackground(_latest.transform.position.x + tileWidth);
        }

        private void SpawnBackground(float x)
        {
            _latest = Instantiate(backgroundTiles[0].tile, new Vector3(x, 5), Quaternion.identity);
        }


        private void AppendTile()
        {
        }
    }
}