using UnityEditor;
using UnityEngine;

namespace Rewrite.UI
{
    public class MainHud : MonoBehaviour
    {
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