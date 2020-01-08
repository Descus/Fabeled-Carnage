using UnityEngine;

[ExecuteAlways]
public class SpriteRandomizer : MonoBehaviour
{
    public Sprite[] lower;

    public SpriteRenderer lowRenderer;
    public Sprite[] upper;
    public SpriteRenderer upRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        upRenderer.sprite = upper[Random.Range(0, upper.Length - 1)];
        lowRenderer.sprite = lower[Random.Range(0, lower.Length - 1)];
    }
}