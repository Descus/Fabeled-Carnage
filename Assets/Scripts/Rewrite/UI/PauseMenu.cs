using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rewrite.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel, countDownPanel ;
        public TextMeshProUGUI countdownText;
        private float count;
        private int seconds;
        private bool countdown;

        private void OnEnable()
        {
            seconds = 3;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)) OpenPause();
            if (countdown)
            {
                count += Time.unscaledDeltaTime;
                if (count >= 1)
                {
                    seconds--;
                    count = 0;
                }
            }
            countdownText.SetText(seconds.ToString());
            if (seconds == 0)
            {
                countDownPanel.SetActive(false);
                gameObject.SetActive(false);
                Time.timeScale = 1;
                countdown = false;
                seconds = 3;
            }

        }

        public void OpenPause()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
            pausePanel.SetActive(true);
        }
        

        public void StartCountdown()
        {
            pausePanel.SetActive(false);
            countDownPanel.SetActive(true);
            countdown = true;
        }

        public void LoadSceneMode(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}
