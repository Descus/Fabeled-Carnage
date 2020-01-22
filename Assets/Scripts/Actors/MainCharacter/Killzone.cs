using UnityEngine;

namespace Actors.MainCharacter
{
    public class Killzone : MonoBehaviour
    {
        public GameObject InKillzone { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            InKillzone = other.gameObject;
            Debug.Log(other.gameObject.name + " entered");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            InKillzone = null;
            Debug.Log(other.gameObject.name + " left");
        }
    }
}