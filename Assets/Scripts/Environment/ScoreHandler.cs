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
        private int score;

        public float scoreFrequency;
        public int scoreMult = 1;

        public int scorePerDistance;
        public TextMeshProUGUI textField;

        private float timeAdder;

        [SerializeField] private int[] combos = {1, 2, 4, 8};
        [SerializeField] private int[] comboTime = { -1, 12, 8, 4};
        private int[] killsToIncrease = {1, 12, 20};
        private int killcount;

        private int _combo;
        public int _comboState;

        public static ScoreHandler Handler;

        public int monoSpacingCharacterSize;

        public int Combo => _combo;

        public Animator comboAnimator;
        public Image comboFiller; 

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

        void Update()
        {
            if (comboTime[_comboState]!=-1)
                if(comboTime[_comboState] >= timeAdder){
                    comboFiller.fillAmount = 1 - comboTime[_comboState] / timeAdder;
                    timeAdder += Time.deltaTime;
                }
                else ResetCombo();
        }

        public void AddScore(int score)
        {
            this.score += score * Combo;
        }

        public void RegisterKill()
        {
            killcount++;
            if (_comboState != 2 && killcount >= killsToIncrease[_comboState])
            {
                IncreaseCombo();
            }
        }

        public void IncreaseCombo()
        {
            _combo = combos[++_comboState];
            _comboState = Mathf.Clamp(_comboState, 0, combos.Length - 1);
            ResetTimer();
        }
        
        public void DecreaseCombo()
        {
            _combo = combos[--_comboState];
            _comboState = Mathf.Clamp(_comboState, 0, combos.Length - 1);
        }

        public void ResetTimer()
        {
            timeAdder = 0;
        }

        public void ResetCombo()
        {
            if (_combo != 1)
            {
                _comboState = 0;
                _combo = combos[_comboState];
                timeAdder = 0;
                killcount = 0;
                Debug.Log("Reset");
                comboAnimator.SetTrigger("Reset");
            }
        }

        private string ConvertToScoreFormat(int score)
        {
            return "<mspace=" + monoSpacingCharacterSize + 
                   ">" + score.ToString().PadLeft(6, '0') + "</mspace>";
        }
    }
}