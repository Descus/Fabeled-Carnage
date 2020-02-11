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
    
        public TextMeshProUGUI ScoreText;
        public TMP_InputField inputField;

        // Start is called before the first frame update

        public void OpenScoreboardScreen()
        {
            
            LoadFile();
            SetUi();
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

            List<GameSave> data = GetSortedList(new GameSave(){score = Score, name = Name});
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
                SetStandardValues();
                return;
            }
 
            BinaryFormatter bf = new BinaryFormatter();
            List<GameSave> data = (List<GameSave>) bf.Deserialize(file);
            file.Close();

            highscores = data;
        }

        private void SetStandardValues()
        {
            foreach (GameSave highscore in highscores)
            {
                highscore.SetName("");;
                highscore.SetScore("0");
            }
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
                highscoreTexts[i].text = i+1 + ". " + highscores[i].name.PadLeft(8) + " " + highscores[i].score.PadLeft(6);
            }
            ScoreText.text = Score;
        }

        public void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}
