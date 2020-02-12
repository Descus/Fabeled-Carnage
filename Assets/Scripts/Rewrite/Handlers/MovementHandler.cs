using Rewrite.GameObjects.Actors.Ambient;
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
        public GameObject meterSign, pfosten;
        public Vector3 meterSignSpawnpos, pfostenSpawnPos;
        private bool spawnFlag;
        private float baseForce;
        private bool spawnPost;

        private void Start()
        {
            Time.timeScale = 1;
            Handler = this;
            wolf = SceneObjectsHandler.Handler.playerObject;
            baseForce = forceField.directionX.constant;
        }

        private void Update()
        {
            GameSpeed = GetGameSpeed(Time.timeSinceLevelLoad);
            forceField.directionX = -GameSpeed * wolf.speed + baseForce;
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
                Instantiate(pfosten, pfostenSpawnPos, Quaternion.identity);
                spawnFlag = true;
                spawnPost = false;
            }

            if (!spawnPost && (int) meterCounter % 100 == 50)
            {
                Instantiate(pfosten, pfostenSpawnPos, Quaternion.identity);
                spawnFlag = false;
                spawnPost = true;
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