using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private float speed = 4f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // calculate bounce direction based on hit position

        if (collision.gameObject.CompareTag("linkerPaddle"))
        {
            float paddleY = collision.gameObject.transform.position.y;

            float hitOffset = transform.position.y - paddleY;

            Vector2 directionHit = new Vector2(+1f, hitOffset);

            directionHit = directionHit.normalized;

            rb.linearVelocity = directionHit * speed;
        }

        if (collision.gameObject.CompareTag("rechterPaddle"))
        {
            float paddleY = collision.gameObject.transform.position.y;

            float hitOffset = transform.position.y - paddleY;

            Vector2 directionHit = new Vector2(-1f, hitOffset);

            directionHit = directionHit.normalized;

            rb.linearVelocity = directionHit * speed;
        }
    }
    void Start()
    {
        // set initial direction and apply velocity to the ball

        float spawnY = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(-1, spawnY);

        direction = direction.normalized;

        rb.linearVelocity = direction * speed;
        
    }
}
