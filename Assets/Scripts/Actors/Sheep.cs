using UnityEngine;

namespace Actors
{
    public class Sheep : Animal
    {
        public override void Move(float speed)
        {
            if (!stunned)
            {
                Transform transform1 = transform;
                Vector3 pos = transform1.position;
                transform1.position = new Vector3(pos.x - speed, pos.y, pos.z);
            }
        }

        protected override void PlayLeapAnim()
        {
        
        }
    }
}
