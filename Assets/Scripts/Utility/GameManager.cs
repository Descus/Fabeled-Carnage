using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}