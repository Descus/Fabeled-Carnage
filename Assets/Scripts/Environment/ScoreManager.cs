using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Environment
{
    public class ScoreManager : MonoBehaviour
    {
        private float _update;
#if UNITY_EDITOR
        [ReadOnly] [SerializeField]
#endif
        private int score;

        public float scoreFrequency;
        public int scoreMult = 1;

        public int scorePerDistance;
        public Text textField;


        // Update is called once per frame
        private void FixedUpdate()
        {
            _update += Time.deltaTime;
            if (_update >= scoreFrequency)
            {
                score += scorePerDistance * scoreMult;
                textField.text = ConvertToScoreFormat(score);
                _update = 0;
            }
        }

        private string ConvertToScoreFormat(int score)
        {
            return score.ToString().PadLeft(8, '0');
        }
    }
}