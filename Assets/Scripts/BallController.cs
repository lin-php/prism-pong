using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private float speed = 4f;

    private Rigidbody2D rb;

    
    void Start()
    {
        // Set initial direction and apply velocity to the ball

        rb = GetComponent<Rigidbody2D>();
        float spawnY = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(-1, spawnY);

        direction = direction.normalized;

        rb.linearVelocity = direction * speed;
        
    }

    
    void Update()
    {
        
    }
}
