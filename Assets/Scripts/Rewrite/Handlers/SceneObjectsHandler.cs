using Rewrite.GameObjects.MainCharacter;
using UnityEngine;

namespace Rewrite.Handlers
{
    public class SceneObjectsHandler: MonoBehaviour
    {
        public static SceneObjectsHandler Handler;

        public Camera mainCamera;
        public Wolf playerObject;
        public GameObject mainHud;
        public RectTransform scoreParent;

        private void Start()
        {
            Handler = this;
        }
    }
}