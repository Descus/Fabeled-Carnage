using UnityEngine;

namespace Rewrite.GameObjects.MainCharacter
{
    public class WolfAnimHandler : MonoBehaviour
    {
        public Wolf wolf;
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
            wolf.ReduceGameSpeed();
        }

    }
}
