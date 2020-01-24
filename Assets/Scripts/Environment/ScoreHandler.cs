using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using Utility;

namespace Environment
{
    public class ScoreHandler : MonoBehaviour
    {
        private float _update;
#if UNITY_EDITOR
        [ReadOnly] [SerializeField]
#endif
        public int score;

        public float scoreFrequency;
        public int scoreMult = 1;

        public int scorePerDistance;
        public TextMeshProUGUI textFieldScore, textFieldCombo;

        public float timeAdder;

        [SerializeField] private int[] combos = {1, 2, 4, 8};
        [SerializeField] private int[] comboTime = { -1, 12, 8, 4};
        private int[] killsToIncrease = {1, 12, 20};
        public int killcount;

        public int combo;
        public int comboState;

        public static ScoreHandler Handler;

        public int monoSpacingCharacterSize;

        public int Combo => combo;

        public Animator comboAnimator;
        public Image comboFiller;
        
        private float _filleramount;

        void Start()
        {
            Handler = this;
            combo = combos[0];
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            _update += Time.deltaTime;
            if (_update >= scoreFrequency)
            {
                AddScore(scorePerDistance * scoreMult);
                textFieldScore.text = ConvertToScoreFormat(score);
                _update = 0;
            }
        }

        void Update()
        {
            if (comboState!=0)
                if(comboTime[comboState] >= timeAdder){
                    _filleramount = (timeAdder / (comboTime[comboState]));
                    comboFiller.fillAmount = 1 - _filleramount;
                    timeAdder += Time.deltaTime;
                }
                else DecreaseCombo();
        }

        public void AddScore(int score)
        {
            this.score += score * Combo;
        }

        public void RegisterKill()
        {
            killcount++;
            if (comboState != 3 && killcount >= killsToIncrease[comboState])
            {
                IncreaseCombo();
            }
        }

        public void IncreaseCombo()
        {
            comboAnimator.SetTrigger("Appear");
            combo = combos[++comboState];
            textFieldCombo.text = ConvertToComboFormat(combo);
            comboState = Mathf.Clamp(comboState, 0, combos.Length - 1);
            ResetTimer();
        }
        
        public void DecreaseCombo()
        {
            combo = combos[--comboState];
            if (comboState == 0)
            {
                comboAnimator.SetTrigger("Reset");
            }
            textFieldCombo.text = ConvertToComboFormat(combo);
            comboState = Mathf.Clamp(comboState, 0, combos.Length - 1);
            killcount = 0;
            ResetTimer();
        }

        public void ResetTimer()
        {
            timeAdder = 0;
        }

        public void ResetCombo()
        {
            if (combo != 1)
            {
                Debug.Log("reset");
                comboState = 0;
                combo = combos[comboState];
                ResetTimer();
                killcount = 0;
                comboAnimator.SetTrigger("Reset");
            }
        }

        private string ConvertToScoreFormat(int score)
        {
            return "<mspace=" + monoSpacingCharacterSize + 
                   ">" + score.ToString().PadLeft(6, '0') + "</mspace>";
        }

        private string ConvertToComboFormat(int combo)
        {
            return combo.ToString().PadRight(2, 'x');
        }
    }
}