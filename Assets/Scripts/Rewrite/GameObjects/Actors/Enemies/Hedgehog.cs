using Rewrite.GameObjects.MainCharacter;
using Rewrite.Handlers;
using UnityEngine;

namespace Rewrite.GameObjects.Actors.Enemies
{
    public class Hedgehog: Animal
    {
        private bool _isRolling;
        public float staminaLoss, rollingTime, walkingtime, walkingspeed;
        private float _lastStateChange;

        public BoxCollider2D rollCollider, walkCollider;

        new void Start()
        {
            base.Start();
            _isRolling = true;
            _lastStateChange = CreationTime;
        }
        
        new void Update()
        {
            if (CanChangeState())
            {
                _lastStateChange = Time.time;
                _isRolling ^= true;
                Speed = GetSpeedForState();
            }

            rollCollider.enabled = _isRolling;
            walkCollider.enabled = !_isRolling;
            animator.SetBool("isRolling", _isRolling);
            base.Update();
        }
        
        private float GetSpeedForState()
        {
            return _isRolling ? baseSpeed : walkingspeed;
        }
        
        private bool CanChangeState()
        {
            return _lastStateChange + GetTimeInState() <= Time.time;
        }
        
        private float GetTimeInState()
        {
            return _isRolling ? rollingTime : walkingtime;
        }
        
        public override bool Push(int lane, float distance)
        {
            if (base.Push(lane, distance))
            {
                if (_isRolling)
                {
                    ScoreHandler.Handler.ResetCombo();
                    SceneObjectsHandler.Handler.playerObject.ReduceStamina(staminaLoss);
                }
                return true;
            }
            return false;
        }
        
        public override bool Kill(GameObject killer)
        {
            if (!killer.gameObject.GetComponent<MovementHandler>())
            {
                killer.GetComponent<Wolf>().ReduceStamina(staminaLoss);
                return base.Kill(killer);
            }
            return base.Kill(killer);
            
        }
    }
}