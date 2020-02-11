using System.Collections;
using UnityEngine;


namespace Rewrite.Handlers
{
    public class ScreenHandler : MonoBehaviour
    {
        public new Camera camera;

        public void ShakeScreen(float length, Vector2 magnitude)
        {
            StartCoroutine(Shake(length, magnitude));

        }

        public void ScreenBreak()
        {
        
        }

        private IEnumerator Shake(float length, Vector2 magnitude)
        {
            Vector3 originalPos = camera.transform.localPosition;
            float timeElapsed = 0.0f;

            while (timeElapsed < length)
            {
                float x = Random.Range(-1f, 1f) * magnitude.x;
                float y = Random.Range(-1f, 1f) * magnitude.y;
                
                camera.transform.localPosition = new Vector3(x, y, originalPos.z);

                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            camera.transform.localPosition = originalPos;
        }
        
        private IEnumerator Break(float length, float intensity)
        {
            Vector3 originalPos = camera.transform.localPosition;
            float timeElapsed = 0.0f;

            while (timeElapsed < length)
            {
                float x = intensity;
                
                camera.transform.localPosition = new Vector3(x, originalPos.y, originalPos.z);

                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            camera.transform.localPosition = originalPos;
        }
        

    }
}
