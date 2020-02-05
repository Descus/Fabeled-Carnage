using UnityEngine;

namespace Rewrite.GameObjects.Actors.Enemies
{
    public class Goose: Animal
    {
        private float _yPos;
        new void Start()
        {
            _yPos = transform.position.y;
            base.Start();
        }
        
        
        new void Update()
        {
            Vector3 position = transform.position;
            position = new Vector3(position.x, _yPos + 0.08f * Mathf.Sin(4 * Time.time), position.z);
            transform.position = position;
            base.Update();
        }
    }
}