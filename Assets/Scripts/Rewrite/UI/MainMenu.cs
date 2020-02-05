using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject MenuPageMain;
        public GameObject MenuPageOptions;

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void OpenOptionMenu()
        {
            MenuPageMain.SetActive(false);
            MenuPageOptions.SetActive(true);
        }

        public void CloseOptionMenu()
        {
            MenuPageMain.SetActive(true);
            MenuPageOptions.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}