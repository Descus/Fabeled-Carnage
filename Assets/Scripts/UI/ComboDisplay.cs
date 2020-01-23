using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace UI
{
    
    public class ComboDisplay : MonoBehaviour
    {
        public GameObject comboCounter;
        public Animator animator;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) animator.SetTrigger("Appear");
            if (Input.GetKeyDown(KeyCode.DownArrow)) animator.SetTrigger("Reset");
            
        }

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
