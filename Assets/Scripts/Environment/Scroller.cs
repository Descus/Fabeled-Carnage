using System;
using UnityEngine;
using Utility;

namespace Environment
{
    public class Scroller : MonoBehaviour
    {
        

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public float gameSpeed;

        public float slowAmount;

        public float speed = -1.5f;

        public ParticleSystemForceField forceField;
        

        private void Update()
        {
            gameSpeed = GetGameSpeed(Time.time);
            forceField.directionX = -gameSpeed * speed;
            if (Input.GetKeyDown(KeyCode.D))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Time.timeScale != 0.1f)
                    Time.timeScale = 0.1f;
                else
                    Time.timeScale = 1f;
            }
            
            EventHandler.BroadcastActorMove(speed * gameSpeed);
            EventHandler.BroadcastBackgroundMove(speed * (1 - slowAmount) * gameSpeed);
        }

        private float GetGameSpeed(float time)
        {
            /*if (time < 30) return 0.002f * time + comboInc + 1;
            if (time < 40) return 0.016f * time + comboInc + 0.72f;
            if (time < 55) return 0.02f * time + comboInc + 0.6f;
            if (time < 65) return 0.04f * time + comboInc - 0.2f;
            if (time < 75) return 0.2f * time + comboInc - 7.4f;
            return 0.02f * time + comboInc + 1.6f;*/
            return Mathf.Sqrt(time)/(9- ScoreHandler.Handler.comboState) + 1;
        }

        
    }
}