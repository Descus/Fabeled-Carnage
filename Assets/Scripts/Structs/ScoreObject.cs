using System;
using UnityEngine;

namespace Structs
{
    public struct ScoreObject
    {
        private int _score;
        private String _name;
        
        public int Score => _score;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public ScoreObject(int score)
        {
            _score = score;
            _name = "";
        }

        public ScoreObject(int score, String name) : this(score)
        {
            _name = name;
        }

        public ScoreObject ParsefromJSonObject(String jsonString)
        {
            return JsonUtility.FromJson<ScoreObject>(jsonString);
        }

        public String WriteToJSonObject()
        {
            return JsonUtility.ToJson(this);
        }

        public override string ToString()
        {
            return Name + " | " + Score;
        }
    }
}