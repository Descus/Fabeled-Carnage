using Actors.MainCharacter;
using Environment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Actors.Enemy
{
    public class Hedgehog : Animal
    {
        
        private bool _isRolling;
        public float staminaLoss;
        public float rollingTime;
        public float walkingtime;
        private float _lastStateChange;
        [SerializeField] private float walkingspeed;
        protected override void PlayLeapAnim()
        {
            
        }
        
        new void Start()
        {
            base.Start();
            _isRolling = true;
            _lastStateChange = TimeCreation;
        }
        
        new void Update()
        {
            if (CanChangeState())
            {
                _lastStateChange = Time.time - 2;
                _isRolling ^= true;
                Speed = GetSpeedForState();
            }
            base.Update();
        }

        private float GetSpeedForState()
        {
            return _isRolling ? baseSpeed : walkingspeed;
        }

        private bool CanChangeState()
        {
            return _lastStateChange <= Time.time + GetTimeInState();
        }

        private float GetTimeInState()
        {
            return _isRolling ? rollingTime : walkingtime;
        }

        public override void Push(int lane, float distance)
        {
            _isRolling = false;
            _lastStateChange = Time.time;
            base.Push(lane, distance);
        }

        public override bool Kill(GameObject killer)
        {
            if (!_isRolling) return base.Kill(killer);
            killer.GetComponent<Wolf>().ReduceStamina(staminaLoss);
            ScoreHandler.Handler.ResetCombo();
            return false;
        }
    }
}