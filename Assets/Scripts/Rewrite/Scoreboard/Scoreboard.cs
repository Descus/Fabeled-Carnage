using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Rewrite.Scoreboard;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    public List<GameSave> highscores = new List<GameSave>(10);
    public Text[] highscoreTexts = new Text[10];
    
    public int Score;
    public String Name;
    
    public Text ScoreText;
    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        OpenScoreboardScreen();
        SetUi();
    }

    public void OpenScoreboardScreen()
    {
        LoadFile();
    }

    public void CloseScoreboardScreen()
    {
        SaveFile();
        SceneManager.LoadScene(0);
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
        foreach (var dat in data)
        {
            Debug.Log(dat.name);
        }
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

        foreach (var dat in data)
        {
            Debug.Log(dat.name);
        }

        highscores = data;
    }

    public List<GameSave> GetSortedList(GameSave save)
    {
        List<GameSave> scores = highscores;
        scores.Add(save);
        scores = scores.OrderByDescending(s => s.score).ToList();
        foreach (var dat in scores)
        {
            Debug.Log(dat.name);
        }
        return scores.GetRange(0,10);
    }

    void SetUi ()
    {
        for (int i = 0; highscoreTexts.Length > i; i++)
        {
            highscoreTexts[i].text = i+1 + ". " + highscores[i].name.PadLeft(8) + " " + highscores[i].score;
        }
        ScoreText.text = "Your Score: " + Score;
    }
}
