using UnityEngine;
using Utility;

namespace Environment
{
    public class BackgroundSpawner : MonoBehaviour
    {

        public float tileWidth = 13;
        public static float SpawnX ;

        public GameObject[] backgroundTiles;
        // Start is called before the first frame update
        private void Start()
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            SpawnX = ScreenUtil.GetRightScreenBorderX(cam) + tileWidth;
            SpawnBackground(-1);
            SpawnBackground(12);
        }

        private void SpawnBackground(float x)
        {
            Instantiate(backgroundTiles[0], new Vector3(x, 5), Quaternion.identity);
        }
        
        

        private void AppendTile()
        {
            
        }
    }
}