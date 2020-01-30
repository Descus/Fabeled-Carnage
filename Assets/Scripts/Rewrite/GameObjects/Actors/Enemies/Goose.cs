using UnityEngine;

namespace Rewrite.GameObjects.Actors.Enemies
{
    public class Goose: Animal
    {
        new void Update()
        {
            var position = transform.position;
            position = new Vector3(position.x, position.y + 0.8f * Mathf.Sin(Time.time), position.z);
            transform.position = position;
            base.Update();
        }
    }
}