using Rewrite.Handlers;
using UnityEngine;

namespace Rewrite.GameObjects.Actors.Enemies
{
    public class Rabbit : Animal
    {
        private bool _canDodge = true, _dodgeBLerp;
        private Vector3 _dodgeTarget, _dodgeStart;
        public float dodgeDistance;
        private float _dodgeLerpFactor;
        
        new void Start()
        {
            base.Start();
        }
        
        public override bool Kill(GameObject killer)
        {
            if (!killer.gameObject.GetComponent<MovementHandler>())
            {
                if (_canDodge)
                {
                    Dodge();
                    _canDodge = false;
                }

                if (!_dodgeBLerp) return base.Kill(killer);
            }
            else return base.Kill(killer); 
            return false;
        }
        
        private void Dodge()
        {
            Vector3 pos = transform.position;
            _dodgeStart = pos;
            _dodgeTarget = new Vector3(pos.x + dodgeDistance, pos.y);
            _dodgeBLerp = true;
            Leaping = true;
        }
        
        new void Update()
        {
            if (_dodgeBLerp)
            {
                _dodgeLerpFactor += 5 * Time.deltaTime;
                Vector3 niew = Vector3.Lerp(_dodgeStart, _dodgeTarget, _dodgeLerpFactor);
                transform.position = niew;
                if (_dodgeLerpFactor >= 1)
                {
                    _dodgeLerpFactor = 0;
                    _dodgeBLerp = false;
                    Leaping = false;
                }
            }
            base.Update();
        }
    }
}