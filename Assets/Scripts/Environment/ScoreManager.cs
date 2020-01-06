using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Environment
{
    public class ScoreManager : MonoBehaviour
    {
        
        public int scorePerDistance;
        public int scoreMult = 1;
        public float scoreFrequency;
        public Text textField;
        #if UNITY_EDITOR
        [ReadOnly][SerializeField]
        #endif
        private int score = 0;
        private float _update;
        

        // Update is called once per frame
        void FixedUpdate()
        {
            _update += Time.deltaTime;
            if (_update >= scoreFrequency)
            {
                score += scorePerDistance * scoreMult;
                textField.text = ConvertToScoreFormat(score);
                _update = 0;
            }
        }

        String ConvertToScoreFormat(int score)
        {
            return score.ToString().PadLeft(8, '0');
        }
    }
}
