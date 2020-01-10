using Actors.MainCharacter;
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
            if (_lastStateChange <= Time.time + (_isRolling ? rollingTime : walkingtime))
            {
                _lastStateChange = Time.time - 2;
                _isRolling ^= true;
            }
            base.Update();
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
            return false;
        }
    }
}