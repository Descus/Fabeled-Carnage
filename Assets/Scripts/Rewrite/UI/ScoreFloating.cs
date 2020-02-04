using TMPro;
using UnityEngine;

namespace Rewrite.UI
{
    public class ScoreFloating : MonoBehaviour
    {
        public AnimationCurve curve;
        public Vector2 target;
        public Vector2 start;
        public float timeToTarget;
        private float _lerpFactor;

        public TextMeshProUGUI textField;

        void Start()
        {
            target = new Vector2(Screen.width * 0.3f - 100, Screen.height - 80f);
        }

        private void Update()
        {
            _lerpFactor += Time.deltaTime/timeToTarget;
            _lerpFactor = Mathf.Clamp(_lerpFactor, 0, 1);
            transform.localPosition = Vector2.Lerp(start, target, curve.Evaluate(_lerpFactor));
            if(_lerpFactor >= 1) Destroy(gameObject);
        }
        
    }
}
