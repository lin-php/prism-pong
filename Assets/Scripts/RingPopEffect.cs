using UnityEngine;

public class RingPopEffect : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        Destroy(gameObject, 0.7f);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.material.color = color;
        spriteRenderer.material.SetColor("_EmissionColor", color * 5f);
    }

}
