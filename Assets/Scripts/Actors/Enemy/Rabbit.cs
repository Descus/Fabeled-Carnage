using Environment;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Actors.Enemy
{
    public class Rabbit : Animal
    {
        private bool _canDodge = true;
        private Vector3 _dodgeTarget;
        private float _dodgeLerpfac;
        private bool _dodgeBLerp;
        public float dodgeDistance = 10;
        private Vector3 _dodgeStart;

        protected override void PlayLeapAnim()
        {
            
        }

        new void Start()
        {
            base.Start();
        }

        public override bool Kill(GameObject killer)
        {
            if (!killer.gameObject.GetComponent<Scroller>())
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
            leaping = true;
        }

        new void Update()
        {
            if (_dodgeBLerp)
            {
                _dodgeLerpfac += 5 * Time.deltaTime;
                Vector3 niew = Vector3.Lerp(_dodgeStart, _dodgeTarget, _dodgeLerpfac);
                transform.position = niew;
                if (_dodgeLerpfac >= 1)
                {
                    _dodgeLerpfac = 0;
                    _dodgeBLerp = false;
                    leaping = false;
                }
            }
            base.Update();
        }
    }
}
