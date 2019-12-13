using UnityEngine;

[ExecuteAlways]
public class SpriteRandomizer : MonoBehaviour
{
    public Sprite[] upper;
    public Sprite[] lower;

    public SpriteRenderer lowRenderer;
    public SpriteRenderer upRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        upRenderer.sprite = upper[Random.Range(0, upper.Length - 1)];
        lowRenderer.sprite = lower[Random.Range(0, lower.Length - 1)];
    }
}
