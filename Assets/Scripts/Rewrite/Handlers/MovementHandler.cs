using Rewrite.GameObjects.MainCharacter;
using UnityEngine;

namespace Rewrite.Handlers
{
    public class MovementHandler: MonoBehaviour
    {
        public static MovementHandler Handler;

        public static float GameSpeed;

        private Wolf wolf;

        public ParticleSystemForceField forceField;
        private float meterCounter;
        public GameObject meterShield;
        public Vector3 meterShieldSpawnpos;

        private void Start()
        {
            Handler = this;
            wolf = SceneObjectsHandler.Handler.playerObject;
        }

        private void Update()
        {
            GameSpeed = GetGameSpeed(Time.time);
            forceField.directionX = -GameSpeed * wolf.speed;
            IncreaseMeterCounter(GameSpeed * wolf.speed);
            EventHandler.BroadcastActorMove(wolf.speed * GameSpeed);
            EventHandler.BroadcastBackgroundMove(wolf.speed * (1 - wolf.slowAmount) * GameSpeed);
        }

        void IncreaseMeterCounter(float inc)
        {
            meterCounter += inc;
            if ((int)meterCounter % 100 == 0)
            {
                Instantiate(meterShield, meterShieldSpawnpos, Quaternion.identity);
            }
        }
        

        private float GetGameSpeed(float time)
        {
            return Mathf.Sqrt(time) / (9 - ScoreHandler.Handler.comboState) + 1;
        }
        
        public void ReduceGameSpeed()
        {
            Time.timeScale = 0.3f;
        }
    }
}