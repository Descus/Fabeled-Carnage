using System;
using Rewrite.Handlers;
using UnityEngine;

namespace Rewrite.GameObjects.MainCharacter
{
    public class WolfAnimHandler : MonoBehaviour
    {
        public Wolf wolf;
        public float fadeValue = 1;
        private bool startReduce;

        private void Update()
        {
            if(startReduce) MovementHandler.Handler.ReduceGameSpeed(fadeValue);
        }

        void KillFrame()
        {
            wolf.HandleKill();
        }

        public void DisplayGameOverScreen()
        {
            wolf.DisplayGameOverScreen();
        }

        public void ReduceGameSpeed()
        {
            startReduce = true;
        }

    }
}
