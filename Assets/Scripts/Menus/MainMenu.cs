using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject MenuPageMain;
        public GameObject MenuPageOptions;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

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