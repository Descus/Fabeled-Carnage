using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rewrite.Handlers
{
    public class ScoreHandler: MonoBehaviour
    {
        private float _filleramount, timeAdder;

        public int score, scoreMult, combo, comboState;
        private int _killcount;

        private int[] combos = {1, 2, 4, 8};
        private int[] comboTime = { -1, 12, 8, 4};
        private int[] killsToIncrease = {1, 12, 20};
        
        public TextMeshProUGUI textFieldScore, textFieldCombo;
        public Image comboFiller;
        public Animator comboAnimator;

        public static ScoreHandler Handler;

        public int monoSpacingCharacterSize;

        private void Start()
        {
            Handler = this;
            combo = combos[0];
        }

        private void FixedUpdate()
        {
            OnScoreFrequencyReach();
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
        
        public void DecreaseCombo()
        {
            combo = combos[--comboState];
            if (comboState == 0)
            {
                comboAnimator.SetTrigger("Reset");
            }
            textFieldCombo.text = ConvertToComboFormat(combo);
            comboState = Mathf.Clamp(comboState, 0, combos.Length - 1);
            ResetKillcount();
            ResetTimer();
        }
        
        public void RegisterKill()
        {
            _killcount++;
            if (comboState != 3 && _killcount >= killsToIncrease[comboState])
            {
                IncreaseCombo();
            }
        }
        
        public void IncreaseCombo()
        {
            comboAnimator.SetTrigger("Appear");
            //StartCoroutine(cameraShaker.Shake(duration, magnitude));
            combo = combos[++comboState];
            textFieldCombo.text = ConvertToComboFormat(combo);
            comboState = Mathf.Clamp(comboState, 0, combos.Length - 1);
            ResetTimer();
        }

        private void ResetKillcount()
        {
            _killcount = 0;
        }

        public void ResetTimer()
        {
            timeAdder = 0;
        }

        private string ConvertToComboFormat(int i)
        {
            return combo.ToString().PadRight(2, 'x');
        }

        private void OnScoreFrequencyReach()
        {
           
        }
        
        public void ResetCombo()
        {
            if (combo != 1)
            {
                Debug.Log("reset");
                comboState = 0;
                combo = combos[comboState];
                ResetTimer();
                ResetKillcount();
                comboAnimator.SetTrigger("Reset");
            }
        }

        public string ConvertToScoreFormat(int i)
        {
            return "<mspace=" + monoSpacingCharacterSize + 
                   ">" + score.ToString().PadLeft(6, '0') + "</mspace>";
        }

        public int AddScore(int score)
        {
            int add = score * combo;
            this.score += add;
            textFieldScore.text = ConvertToScoreFormat(score);
            return add;
        }
    }
}