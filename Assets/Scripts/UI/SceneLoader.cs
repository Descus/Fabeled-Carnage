using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
            Debug.Log("AAAHHHH");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
