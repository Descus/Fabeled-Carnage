
using Actors.MainCharacter;
using Environment;
using UnityEngine;
using Utility;

namespace Actors.Enemy
{
    public class Hedgehog : Animal
    {
        [SerializeField] [ReadOnly]
        private bool isRolling;
        public float staminaLoss;
        public float rollingTime;
        public float walkingtime;
        private float _lastStateChange;

        public BoxCollider2D rollCollider;
        public BoxCollider2D walkCollider;

        [SerializeField] private float walkingspeed = 0.5f;
        protected override void PlayLeapAnim()
        {
            
        }
        
        new void Start()
        {
            base.Start();
            isRolling = true;
            _lastStateChange = TimeCreation;
        }
        
        new void Update()
        {
            if (CanChangeState())
            {
                _lastStateChange = Time.time;
                isRolling ^= true;
                Speed = GetSpeedForState();
            }

            rollCollider.enabled = isRolling;
            walkCollider.enabled = !isRolling;
            animator.SetBool("isRolling", isRolling);
            base.Update();
        }

        private float GetSpeedForState()
        {
            return isRolling ? baseSpeed : walkingspeed;
        }

        private bool CanChangeState()
        {
            return _lastStateChange + GetTimeInState() <= Time.time;
        }

        private float GetTimeInState()
        {
            return isRolling ? rollingTime : walkingtime;
        }

        public override bool Push(int lane, float distance)
        {
            if (base.Push(lane, distance))
            {
                isRolling = false;
                _lastStateChange = Time.time;
                return true;
            }
            return false;
        }

        public override bool Kill(GameObject killer)
        {
            if (!killer.gameObject.GetComponent<Scroller>())
            {
                if (!isRolling) return base.Kill(killer);
                killer.GetComponent<Wolf>().ReduceStamina(staminaLoss);
                ScoreHandler.Handler.ResetCombo();
            }
            else return base.Kill(killer);
            return false;
        }
    }
}