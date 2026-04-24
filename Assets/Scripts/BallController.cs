using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public float speed = 3.5f;
    [SerializeField] private GameManager gameManager;
    [Space (20)]
    [SerializeField] private AudioClip paddleHit;
    [SerializeField] private AudioClip paddleAiHit;
    [Space(20)]
    [SerializeField] private Material[] ballcolour;

    private Rigidbody2D rb;
    private float targetSpeed;
    private float newSpeed;
    private SpriteRenderer spriteRenderer;

    private Material currentMaterial;
    public Material CurrentMaterial { get { return currentMaterial; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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

            AudioController.Instance.SoundOnHit(paddleHit, 0.3f);
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
        // set random colour

        SetRandomMaterial();    

        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;   

        float spawnY = Random.Range(-1.6f, 1.6f);
        int randomX = Random.value < 0.5f ? -1 : 1;

        Vector2 direction = new Vector2(randomX, spawnY);

        direction = direction.normalized;

        rb.linearVelocity = direction * speed;
    }


    // random colour on balls

    private void SetRandomMaterial()
    {
        int index = Random.Range(0, ballcolour.Length);
        spriteRenderer.material = ballcolour[index];    
        currentMaterial = spriteRenderer.material;
    }


}
