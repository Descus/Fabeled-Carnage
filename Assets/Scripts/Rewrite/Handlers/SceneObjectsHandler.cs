using Rewrite.GameObjects.MainCharacter;
using UnityEngine;

namespace Rewrite
{
    public class SceneObjectsHandler: MonoBehaviour
    {
        public static SceneObjectsHandler Handler;

        public Camera mainCamera;
        public Wolf playerObject;
        public GameObject mainHud;

        private void Start()
        {
            Handler = this;
        }
    }
}