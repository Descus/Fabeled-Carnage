using TMPro;
using UnityEngine;
using Utility;

namespace Environment
{
    public class ScoreHandler : MonoBehaviour
    {
        private float _update;
#if UNITY_EDITOR
        [ReadOnly] [SerializeField]
#endif
        private int score;

        public float scoreFrequency;
        public int scoreMult = 1;

        public int scorePerDistance;
        public TextMeshProUGUI textField;

        [SerializeField]
        private int[] combos = { 1,2,4,8 };

        private int _combo;
        private int _comboState;

        public static ScoreHandler Handler;

        public int monoSpacingCharacterSize;

        public int Combo => _combo;


        void Start()
        {
            Handler = this;
            _combo = combos[0];
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            _update += Time.deltaTime;
            if (_update >= scoreFrequency)
            {
                AddScore(scorePerDistance * scoreMult);
                textField.text = ConvertToScoreFormat(score);
                _update = 0;
            }
        }

        public void AddScore(int score)
        {
            this.score += score * Combo;
        }

        public void IncreaseCombo()
        {
            _combo = combos[++_comboState];
            _comboState = Mathf.Clamp(_comboState, 0, combos.Length - 1);
        }
        
        public void DecreaseCombo()
        {
            _combo = combos[--_comboState];
            _comboState = Mathf.Clamp(_comboState, 0, combos.Length - 1);
        }

        public void ResetCombo()
        {
            _comboState = 0;
            _combo = combos[_comboState];
        }

        private string ConvertToScoreFormat(int score)
        {
            return "<mspace=" + monoSpacingCharacterSize + 
                   ">" + score.ToString().PadLeft(6, '0') + "</mspace>";
        }
    }
}