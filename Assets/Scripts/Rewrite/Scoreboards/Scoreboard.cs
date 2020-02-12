using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rewrite.Scoreboards
{
    public class Scoreboard : MonoBehaviour
    {
        public List<GameSave> highscores = new List<GameSave>(10);
        public TextMeshProUGUI[] highscoreTexts = new TextMeshProUGUI[10];
    
        public string Score;
        public string Name;

        public GameSave latestSave;
    
        public TextMeshProUGUI ScoreText;
        public TMP_InputField inputField;
        public string numberSpace, textSpace;

        // Start is called before the first frame update

        public void OpenScoreboardScreen()
        {
            LoadFile();
            latestSave = new GameSave() {score = Score, name = Name};
            highscores = GetSortedList(latestSave);
            SetUi();
        }

        private void OnEnable()
        {
            OpenScoreboardScreen();
        }

        public void CloseScoreboardScreen(int followScene)
        {
            SaveFile();
            LoadScene(followScene);
        }

        public void UpdateName()
        {
            Name = inputField.text;
        }

        public void SaveFile()
        {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;
 
            if(File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);
            
            UpdateName();
            List<GameSave> data = highscores;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
    
        public void LoadFile()
        {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;
 
            if(File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                File.Create(destination);
                return;
            }
 
            BinaryFormatter bf = new BinaryFormatter();
            List<GameSave> data = (List<GameSave>) bf.Deserialize(file);
            file.Close();

            highscores = data;
        }

        public void OnType()
        {
            UpdateName();
            latestSave.SetName(Name);
            SetUi();
        }

        public List<GameSave> GetSortedList(GameSave save)
        {
            List<GameSave> scores = highscores;
            scores.Add(save);
            scores = scores.OrderByDescending(s => s.score).ToList();
            return scores.GetRange(0,10);
        }

        void SetUi ()
        {
            for (int i = 0; highscoreTexts.Length > i; i++)
            {
                highscoreTexts[i].SetText(
                    ApplyMonoSpacing((i + 1) + ". ", numberSpace) 
                    + ApplyMonoSpacing(highscores[i].name.PadRight(8), textSpace) 
                    + ApplyMonoSpacing(" " + highscores[i].score.PadLeft(6,'0'), numberSpace)
                    );
            }
            ScoreText.text = Score;
        }

        private string ApplyMonoSpacing(string text, string space)
        {
            return "<mspace=" + space + ">" + text + "</mspace>";
        }


        public void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}
