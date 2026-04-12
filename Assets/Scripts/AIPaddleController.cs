using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;

    [SerializeField] private GameObject ball;
    [SerializeField] private float offsetRange;
    [SerializeField] private float offsetChangeInterval = 5f;
    [SerializeField] private float deadzone = 0.2f;
    [SerializeField] private float offsetSmoothSpeed = 1f;
    

    private float currentOffset;
    private float targetOffset;
    
    private float timer;

    private void Start()
    {
        RandomOffset();
        currentOffset = targetOffset;
        timer = offsetChangeInterval;
    }

    private void RandomOffset()
    {
        // new mistake in random range
        targetOffset = Random.Range(-offsetRange, offsetRange);
    }

    void Update()
    {
        // smooth movement
        currentOffset = Mathf.MoveTowards(currentOffset, targetOffset, offsetSmoothSpeed * Time.deltaTime); 

        // timer for new offset

        timer -= Time.deltaTime;

        if (timer <= 0) 
        {
            RandomOffset();
            timer = offsetChangeInterval;
        }

        // take current position and ball height
        float pos = transform.position.y;
        float ballY = ball.transform.position.y;

        // movement
        float difference = (ballY + currentOffset) - pos;
        float direction = 0f;

        if (difference > deadzone)
        {
             direction = 1f;
        }
        else if (difference < -deadzone)
        {
             direction = -1f;
        }
     
        pos += direction * speed * Time.deltaTime;

        float clampedY = Mathf.Clamp(pos, -4.5f, 4.5f);

        transform.position = new Vector2 (transform.position.x, clampedY);

    }
}
