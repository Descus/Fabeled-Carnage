using Rewrite.GameObjects.MainCharacter;
using Rewrite.Spawning;
using UnityEngine;

namespace Rewrite.Handlers
{
    public class MovementHandler: MonoBehaviour
    {
        public static MovementHandler Handler;

        public float GameSpeed;

        private Wolf wolf;

        public ParticleSystemForceField forceField;
        private float meterCounter;
        public GameObject meterSign;
        public Vector3 meterSignSpawnpos;
        private bool spawnFlag;

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
            meterCounter += inc * Time.deltaTime;
            if (!spawnFlag && (int)meterCounter % 100 == 0 && meterCounter > 50)
            {
                GameObject instance = Instantiate(meterSign, meterSignSpawnpos, Quaternion.identity);
                instance.GetComponent<Sign>().distance = (int)meterCounter;
                spawnFlag = true;
            }

            if ((int) meterCounter % 100 == 50)
            {
                spawnFlag = false;
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