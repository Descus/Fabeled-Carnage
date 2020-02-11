using Interfaces;
using Rewrite.Handlers;
using Rewrite.UI;
using TMPro;
using UnityEngine;

namespace Rewrite.GameObjects.Actors.Ambient
{
    public class Sign : MonoBehaviour, IScrollable
    {
        public int distance;
        public TextMeshPro textField;
        public GameObject scoreFloater;
        public RectTransform scoreParent;
        private bool added;

        // Start is called before the first frame update
        void Start()
        {
            textField.text = distance.ToString();
            scoreParent = SceneObjectsHandler.Handler.scoreParent;
        }

        // Update is called once per frame
        void Update()
        {
            float playerX = SceneObjectsHandler.Handler.playerObject.transform.position.x;
            if (!added && playerX >= transform.position.x)
            {
                AddScore();
            }
        }

        void AddScore()
        {
            Vector3 position = transform.position;
            GameObject instance = Instantiate(scoreFloater, SceneObjectsHandler.Handler.mainCamera.WorldToScreenPoint(position), Quaternion.identity, scoreParent.transform);
            ScoreFloating floater = instance.GetComponent<ScoreFloating>();
            floater.start = SceneObjectsHandler.Handler.mainCamera.WorldToScreenPoint(position);
            floater.textField.text = ScoreHandler.Handler.AddScore(distance).ToString();
            added = true;
        }

        public void Move(float speed)
        {
            Transform trans = transform;
            Vector3 pos = trans.position;
            trans.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z);
            if (transform.position.x <= LaneManager.Manager.DespawnXPos) Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventHandler.SubscribeBackgroundMoveEvent(Move);
        }

        private void OnDisable()
        {
            EventHandler.UnSubscribeBackgroundMoveEvent(Move);
        }

        public void SetSpeedDeviancyforLane(float deviancy, int lane)
        {
        }
    }
}
