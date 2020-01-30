using UnityEngine;

namespace Rewrite.UI
{
    public class ComboDisplay
    {
        public GameObject comboCounter;
        
        public void CreateComboCounter()
        {
            comboCounter.SetActive(true);
        }

        public void DestroyComboCounter()
        {
            comboCounter.SetActive(false);
        }
    }
}