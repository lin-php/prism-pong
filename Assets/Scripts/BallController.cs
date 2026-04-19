using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip paddleHit;
    [SerializeField] private AudioClip paddleAiHit;

    private Rigidbody2D rb;
    private float targetSpeed;
    private float newSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        
        gameManager = FindFirstObjectByType<GameManager>();

        targetSpeed = speed;
    }

    public void IncreaseBallSpeed(float amount)
    {
        speed += amount;
        targetSpeed = speed;
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

            AudioController.Instance.SoundOnHit(paddleHit, 1f);

            gameManager.AddPlayerPointonPaddlehit();

        }

        if (collision.gameObject.CompareTag("rechterPaddle"))
        {
            float paddleY = collision.gameObject.transform.position.y;

            float hitOffset = transform.position.y - paddleY;

            Vector2 directionHit = new Vector2(-1f, hitOffset);

            directionHit = directionHit.normalized;

            rb.linearVelocity = directionHit * speed;

            AudioController.Instance.SoundOnHit(paddleHit, 0.4f);
        }
    }


    // acceleration if ball gets slowed
    private void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed < targetSpeed)
        {
            newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 0.5f * Time.fixedDeltaTime);
            Vector2 direction = rb.linearVelocity.normalized;
            rb.linearVelocity = direction * newSpeed;
        }
    }

    public void SpawnBall()
    {
        // reset position -> middle of the field
        // set initial direction and apply velocity to the ball

        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;   

        float spawnY = Random.Range(-1.5f, 1.5f);

        Vector2 direction = new Vector2(-1, spawnY);

        direction = direction.normalized;

        rb.linearVelocity = direction * speed;
    }

}
