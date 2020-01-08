using UnityEditor;
using UnityEngine;

namespace Utility
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            /*
        if (Application.platform != RuntimePlatform.Android)
        {
            GameObject.Find("btnUp").SetActive(false);
            GameObject.Find("btnDown").SetActive(false);
            GameObject.Find("btnAttack").SetActive(false);
            
        }
        */
        }


        public void OnCloseGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}