using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public float speed = 3.5f;
    [SerializeField] private GameManager gameManager;
    [Space (20)]
    [SerializeField] private AudioClip paddleHit;
    [SerializeField] private AudioClip paddleAiHit;
    [Space(20)]
    [SerializeField] private Color[] ballColors;

    private Rigidbody2D rb;
    private float targetSpeed;
    private float newSpeed;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private SpriteRenderer innerRenderer;
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticleSystemRenderer particleRenderer;

    private Color currentColor;
    public Color CurrentColor { get { return currentColor; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        particleRenderer = _particleSystem.GetComponent<ParticleSystemRenderer>(); 
        
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
            SetRandomMaterial();

        }

        if (collision.gameObject.CompareTag("rechterPaddle"))
        {
            float paddleY = collision.gameObject.transform.position.y;

            float hitOffset = transform.position.y - paddleY;

            Vector2 directionHit = new Vector2(-1f, hitOffset);

            directionHit = directionHit.normalized;

            rb.linearVelocity = directionHit * speed;

            AudioController.Instance.SoundOnHit(paddleHit, 0.5f);
            SetRandomMaterial();
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
        Color newColor;
        int index;

        do
        {
            index = Random.Range(0, ballColors.Length);
            newColor = ballColors[index];

        }
        while (newColor == currentColor);

        currentColor = newColor;

        spriteRenderer.material.color = newColor;
        spriteRenderer.material.SetColor("_EmissionColor", newColor * 5f);

        
        innerRenderer.material.color = newColor;
        innerRenderer.material.SetColor("_EmissionColor", newColor * 0.5f);

        trailRenderer.startColor = newColor;

        float particleEmission = 5.6f;


        // manually adjust blue and purple particle emission because bloom/emission makes it looks wrong
        if (index == 0) 
        {
            Color blueEmission = new Color32(0, 15, 191, 255);
            particleRenderer.material.SetColor("_EmissionColor", blueEmission * 8f);

        }
        else if (index == 5)
        {
            Color purpleEmission = new Color32(29, 0, 191, 255);
            particleRenderer.material.SetColor("_EmissionColor", purpleEmission * 8f);
        }
        else
        {
            particleRenderer.material.SetColor("_EmissionColor", newColor * particleEmission);
        }

        particleRenderer.material.color = newColor;
        
    }
}
