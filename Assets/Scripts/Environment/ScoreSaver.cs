using System;
using Structs;
using UnityEngine;
using Utility;

namespace Environment
{
    public class ScoreSaver: MonoBehaviour
    {
        ScoreObject[] _scoreObjects = new ScoreObject[10];

        void Start()
        {
        }

        public void SortScore(ScoreObject score)
        {
            if (score.Score < _scoreObjects[_scoreObjects.Length - 1].Score)
            {
                return;
            }
            if (score.Score > _scoreObjects[0].Score)
            {
                _scoreObjects = Utilities.ShiftDown(_scoreObjects, 0);
                _scoreObjects[0] = score;
            } 
            else for (int i = _scoreObjects.Length - 1; i > 0; i--)
            {
                if (_scoreObjects[i - 1].Score >= score.Score && _scoreObjects[i].Score < score.Score)
                {
                    _scoreObjects = Utilities.ShiftDown(_scoreObjects, i);
                    _scoreObjects[i] = score;
                }
            }
        }
    }
}