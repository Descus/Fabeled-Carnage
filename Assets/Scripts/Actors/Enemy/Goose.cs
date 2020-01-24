using UnityEngine;

namespace Actors.Enemy
{
    public class Goose : Animal
    {
        // Update is called once per frame
        new void Update()
        {
            var position = transform.position;
            position = new Vector3(position.x, position.y + 0.8f * Mathf.Sin(Time.time), position.z);
            transform.position = position;
            base.Update();
        }

        protected override void PlayLeapAnim()
        {
            
        }
    }
}
