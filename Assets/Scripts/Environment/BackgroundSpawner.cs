using Structs;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Utility;

namespace Environment
{
    public class BackgroundSpawner : MonoBehaviour
    {

        public float tileWidth = 16;
        public static float SpawnX ;
        private GameObject latest;

        public BackgroundTile[] backgroundTiles;
        // Start is called before the first frame update
        private void Start()
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            SpawnX = ScreenUtil.GetRightScreenBorderX(cam) + 1;
            SpawnBackground(0);
            SpawnBackground(16);
        }

        void Update()
        {
            if (latest.transform.position.x <= tileWidth)
            {
                SpawnBackground(latest.transform.position.x + tileWidth);
            }
        }

        private void SpawnBackground(float x)
        {
            latest = Instantiate(backgroundTiles[0].tile, new Vector3(x, 5), Quaternion.identity);
        }
        
        

        private void AppendTile()
        {
            
        }
    }
}