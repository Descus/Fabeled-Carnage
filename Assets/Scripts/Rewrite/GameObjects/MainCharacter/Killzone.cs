using UnityEngine;

namespace Rewrite.GameObjects.MainCharacter
{
    public class Killzone: MonoBehaviour
    {
        public GameObject InKillzone { get; private set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (InKillzone == null) InKillzone = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            InKillzone = null;
        }
    }
}