using System;
using UnityEngine;

namespace Actors
{
    public class Killzone: MonoBehaviour
    {
        private GameObject inKillzone;

        public GameObject InKillzone => inKillzone;

        private void OnTriggerEnter2D(Collider2D other)
        {
            inKillzone = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            inKillzone = null;
        }
    }
}